using System.Collections.Generic;
using UnityEngine;

public class CellGame : MonoBehaviour {
    class Unit { public Vector2 p; public float hp; public int kind; public float timer; }
    readonly List<Unit> enemies = new List<Unit>();
    readonly List<Vector2> shots = new List<Vector2>();
    Vector2 player = new Vector2(0,-6);
    Texture2D pixel;
    float spawn, fire, elapsed;
    int hp = 100, energy = 0, level = 1, kills = 0;
    bool ended;
    void Start() {
        Application.targetFrameRate = 60;
        Screen.orientation = ScreenOrientation.Portrait;
        pixel = new Texture2D(1,1);
        pixel.SetPixel(0,0,Color.white); pixel.Apply();
    }
    void Update() {
        if (ended) { if (Input.GetMouseButtonDown(0)) Restart(); return; }
        float dt = Mathf.Min(Time.deltaTime,0.05f); elapsed += dt;
        if (Input.GetMouseButton(0)) {
            Vector3 w = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            player = Vector2.MoveTowards(player, new Vector2(Mathf.Clamp(w.x,-4.1f,4.1f),Mathf.Clamp(w.y,-7.2f,7.2f)),dt*18f);
        }
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        player += Vector2.ClampMagnitude(move,1)*dt*7f;
        player.x=Mathf.Clamp(player.x,-4.1f,4.1f);player.y=Mathf.Clamp(player.y,-7.2f,7.2f);
        fire-=dt; if(fire<=0){fire=0.22f;shots.Add(player+Vector2.up*0.45f);}
        spawn-=dt;
        if(spawn<=0) {spawn=Mathf.Max(0.32f,1.1f-elapsed*0.004f); enemies.Add(new Unit {p=new Vector2(Random.Range(-4f,4f),9),hp=2+level,kind=Random.Range(0,7)});}
        for(int i=shots.Count-1;i>=0;i--) {shots[i]+=Vector2.up*dt*13f;if(shots[i].y>10)shots.RemoveAt(i);}
        for(int i=enemies.Count-1;i>=0;i--) {
            var e=enemies[i]; e.timer+=dt;
            float x=(e.kind==0?Mathf.Sin(e.timer*3)*1.5f:0);
            e.p+=new Vector2(x*dt,-(e.kind==2?3.5f:2.1f)*dt);
            if(e.kind==3 && Mathf.Abs(player.x-e.p.x)<1.5f)e.p+=Vector2.up*dt;
            if(Vector2.Distance(e.p,player)<0.58f) {hp-=20;enemies.RemoveAt(i);if(hp<=0)ended=true;continue;}
            bool dead=false;
            for(int j=shots.Count-1;j>=0;j--)if(Vector2.Distance(e.p,shots[j])<0.5f){shots.RemoveAt(j);e.hp--;if(e.hp<=0){dead=true;break;}}
            if(dead){enemies.RemoveAt(i);kills++;energy+=10;if(energy>=level*50){energy-=level*50;level++;hp=Mathf.Min(100,hp+15);} }
            else if(e.p.y < -10)enemies.RemoveAt(i);
        }
    }
    void Restart(){enemies.Clear();shots.Clear();hp=100;energy=0;level=1;kills=0;elapsed=0;ended=false;player=new Vector2(0,-6);}
    void Draw(Vector2 p,float w,float h,Color c){
        GUI.color=c; Vector3 s=Camera.main.WorldToScreenPoint(new Vector3(p.x,p.y,0));
        float scale=Screen.height/18f;
        GUI.DrawTexture(new Rect(s.x-w*scale/2,Screen.height-s.y-h*scale/2,w*scale,h*scale),pixel);
        GUI.color=Color.white;
    }
    void OnGUI(){
        if(pixel==null||Camera.main==null)return;
        Draw(player,0.65f,0.8f,Color.cyan);
        foreach(var s in shots)Draw(s,0.12f,0.4f,Color.yellow);
        foreach(var e in enemies)Draw(e.p,e.kind==6?1.2f:0.65f,e.kind==6?1.2f:0.65f,e.kind==5?Color.magenta:Color.red);
        GUI.skin.label.fontSize=Mathf.Max(18,Screen.width/27);
        GUI.color=Color.white;
        GUI.Label(new Rect(12,12,Screen.width-20,60),"HP "+hp+"   LV "+level+"   ENERGY "+energy+"   KILLS "+kills);
        if(ended){GUI.skin.label.fontSize=Screen.width/12;GUI.Label(new Rect(Screen.width*0.18f,Screen.height*0.45f,Screen.width,100),"CELL LOST");GUI.Label(new Rect(20,Screen.height*0.55f,Screen.width,70),"Tap to restart");}
    }
}
