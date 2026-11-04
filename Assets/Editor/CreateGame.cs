using System.IO;
using CellDefender;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
public static class CreateGame {
 static Sprite sprite;
 static Sprite Pixel(){
  if(sprite!=null)return sprite;
  const string path="Assets/Generated/pixel.png";
  Directory.CreateDirectory("Assets/Generated");
  if(!File.Exists(path)){
   var t=new Texture2D(1,1,TextureFormat.RGBA32,false);
   t.SetPixel(0,0,Color.white);t.filterMode=FilterMode.Point;t.Apply();
   File.WriteAllBytes(path,t.EncodeToPNG());
   Object.DestroyImmediate(t);
  }
  AssetDatabase.ImportAsset(path,ImportAssetOptions.ForceSynchronousImport);
  var importer=AssetImporter.GetAtPath(path) as TextureImporter;
  if(importer!=null&&(importer.textureType!=TextureImporterType.Sprite||importer.filterMode!=FilterMode.Point)){
   importer.textureType=TextureImporterType.Sprite;importer.spritePixelsPerUnit=1;
   importer.filterMode=FilterMode.Point;importer.mipmapEnabled=false;
   importer.SaveAndReimport();
  }
  sprite=AssetDatabase.LoadAssetAtPath<Sprite>(path);
  if(sprite==null)throw new System.Exception("Persistent sprite import failed: "+path);
  return sprite;
 }
 static GameObject Shape(string name,Vector3 pos,Vector2 size,Color color,bool collider=true){
  var go=new GameObject(name);go.transform.position=pos;go.transform.localScale=new Vector3(size.x,size.y,1);
  var sr=go.AddComponent<SpriteRenderer>();sr.sprite=Pixel();sr.color=color;
  if(collider){var box=go.AddComponent<BoxCollider2D>();box.isTrigger=true;var rb=go.AddComponent<Rigidbody2D>();rb.bodyType=RigidbodyType2D.Kinematic;rb.gravityScale=0;}
  return go;
 }
 public static void Generate(){
  Directory.CreateDirectory("Assets/Scenes");
  var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
  var cam=new GameObject("Main Camera").AddComponent<Camera>();
  cam.tag="MainCamera";cam.orthographic=true;cam.orthographicSize=9;
  cam.backgroundColor=new Color(.045f,.07f,.12f);cam.transform.position=new Vector3(0,0,-10);
  var session=new GameObject("Game Session").AddComponent<GameSession>();session.StartRun();
  new GameObject("HUD").AddComponent<GameHud>();
  var player=Shape("Player",new Vector3(0,-6,0),new Vector2(.7f,.7f),Color.cyan);
  player.tag="Player";var pr=player.AddComponent<PlayerRuntime>();player.AddComponent<PlayerDamageReceiver>();
  var shot=Shape("Player Bullet Template",new Vector3(0,100,0),new Vector2(.13f,.4f),Color.yellow);
  shot.AddComponent<ProjectileRuntime>().hostile=false;pr.shotPrefab=shot;
  var enemy=Shape("Enemy Template",new Vector3(0,100,0),new Vector2(.7f,.7f),Color.magenta);
  enemy.AddComponent<EnemyRuntime>();
  var bullet=Shape("Enemy Bullet Template",new Vector3(0,100,0),new Vector2(.15f,.3f),Color.red);
  bullet.AddComponent<ProjectileRuntime>();
  shot.SetActive(false);enemy.SetActive(false);bullet.SetActive(false);
  var bootstrap=new GameObject("Runtime Bootstrap").AddComponent<RuntimeBootstrap>();
  bootstrap.player=pr;bootstrap.enemyTemplate=enemy;bootstrap.enemyBulletTemplate=bullet;bootstrap.playerBulletTemplate=shot;
  EditorSceneManager.SaveScene(scene,"Assets/Scenes/Main.unity");
  AssetDatabase.SaveAssets();
 }
}
