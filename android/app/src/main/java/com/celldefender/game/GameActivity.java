package com.celldefender.game;

import android.app.Activity;
import android.os.Bundle;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.webkit.WebView;
import android.webkit.WebChromeClient;
import androidx.webkit.WebViewAssetLoader;
import androidx.webkit.WebViewAssetLoader.AssetsPathHandler;
import androidx.webkit.WebViewClientCompat;
import android.webkit.WebResourceRequest;
import android.webkit.WebResourceResponse;

public class GameActivity extends Activity implements SensorEventListener {
 private WebView web;
 private SensorManager sensors;
 private Sensor rotation;
 private boolean ready = false;
 private long lastSensorUpdateNs = 0L;
 private final float[] matrix = new float[9];
 private final float[] orientation = new float[3];
 @Override public void onCreate(Bundle state) {
  super.onCreate(state);
  getWindow().getDecorView().setSystemUiVisibility(5894 | 1024 | 512);
  sensors = (SensorManager)getSystemService(SENSOR_SERVICE);
  rotation = sensors == null ? null : sensors.getDefaultSensor(Sensor.TYPE_ROTATION_VECTOR);
  web = new WebView(this);
  web.getSettings().setJavaScriptEnabled(true);
  web.getSettings().setDomStorageEnabled(true);
  web.getSettings().setMediaPlaybackRequiresUserGesture(true);
  web.setWebChromeClient(new WebChromeClient());
  WebViewAssetLoader loader = new WebViewAssetLoader.Builder()
    .addPathHandler("/assets/", new AssetsPathHandler(this)).build();
  web.setWebViewClient(new WebViewClientCompat() {
   @Override public WebResourceResponse shouldInterceptRequest(WebView view, WebResourceRequest request) {
    return loader.shouldInterceptRequest(request.getUrl());
   }
   @Override public void onPageFinished(WebView view, String url) {view.evaluateJavascript("window.cellNativeSensorAvailable="+(rotation!=null)+";", value -> ready=true);}
  });
  setContentView(web);
  ready=false;
  web.loadUrl("https://appassets.androidplatform.net/assets/game/index.html");
 }
 @Override protected void onResume(){lastSensorUpdateNs=0L;super.onResume();if(sensors!=null&&rotation!=null)sensors.registerListener(this,rotation,SensorManager.SENSOR_DELAY_GAME);if(web!=null)web.onResume();}
 @Override protected void onPause(){if(sensors!=null)sensors.unregisterListener(this);if(web!=null)web.onPause();super.onPause();}
 @Override protected void onDestroy(){if(web!=null){web.destroy();web=null;}super.onDestroy();}
 @Override public void onSensorChanged(SensorEvent event){
  if(!ready||web==null||event.sensor.getType()!=Sensor.TYPE_ROTATION_VECTOR)return;
  if(event.timestamp-lastSensorUpdateNs<33_000_000L)return;
  lastSensorUpdateNs=event.timestamp;
  SensorManager.getRotationMatrixFromVector(matrix,event.values);
  SensorManager.getOrientation(matrix,orientation);
  final double alpha=Math.toDegrees(orientation[0]);
  final double gamma=Math.toDegrees(orientation[2]);
  if(!Double.isFinite(alpha)||!Double.isFinite(gamma))return;
  final String js="window.cellNativeOrientation&&window.cellNativeOrientation("+gamma+","+alpha+");";
  web.post(()->{if(web!=null)web.evaluateJavascript(js,null);});
 }
 @Override public void onAccuracyChanged(Sensor sensor,int accuracy){}
 @Override public void onBackPressed(){if(web==null)return;web.evaluateJavascript("if(typeof g!=='undefined'&&g.state==='playing'){g.pause();ui();}else{history.back();}",null);}
}
