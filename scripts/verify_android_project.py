#!/usr/bin/env python3
"""Offline Android packaging and integration preflight; not an APK compilation."""
from pathlib import Path
import re,sys,xml.etree.ElementTree as ET
root=Path(__file__).resolve().parents[1]
errors=[]
def require(ok,msg):
    if not ok:errors.append(msg)
manifest=root/'android/app/src/main/AndroidManifest.xml'
try:
    m=ET.parse(manifest).getroot()
    ns='{http://schemas.android.com/apk/res/android}'
    app=m.find('application'); activity=app.find('activity') if app is not None else None
    require(activity is not None,'Missing activity')
    if activity is not None:
        require(activity.get(ns+'exported')=='true','Launcher not exported')
        require(activity.get(ns+'screenOrientation')=='portrait','Not portrait')
except Exception as e:errors.append('Manifest XML: '+str(e))
gradle=(root/'android/app/build.gradle').read_text()
require('com.android.application' in gradle,'Missing application plugin')
require('compileSdk 35' in gradle,'Unexpected compileSdk')
require('androidx.webkit:webkit:' in gradle,'Missing WebViewAssetLoader dependency')
java=(root/'android/app/src/main/java/com/celldefender/game/GameActivity.java').read_text()
require('WebViewAssetLoader' in java and 'appassets.androidplatform.net/assets/game/index.html' in java,'WebView asset origin mismatch')
require('window.cellNativeOrientation' in java,'Native sensor callback missing')
require('Sensor.TYPE_ROTATION_VECTOR' in java,'Rotation vector missing')
require('unregisterListener(this)' in java,'Sensor lifecycle cleanup missing')
web=(root/'game/web/index.html').read_text()
require('window.cellNativeOrientation=' in web,'Web native callback handler missing')
require('window.cellNativeSensorAvailable' in web,'Web sensor availability gate missing')
for file in ['index.html','game.js','controls.js']:
    require((root/'android/app/src/main/assets/game'/file).is_file(),f'Bundled {file} missing')
workflow=(root/'.github/workflows/android-debug-apk.yml').read_text()
require('assembleDebug' in workflow and 'upload-artifact@v4' in workflow,'CI build/upload incomplete')
require('verify_android_assets.py' in workflow,'CI missing asset integrity test')
print('ANDROID_PREFLIGHT', 'PASS' if not errors else 'FAIL', 'checks=15', 'errors=',errors)
sys.exit(bool(errors))
