#!/usr/bin/env python3
"""Deterministic CI fallback resources. Does not replace or claim to match Stage 15 artwork."""
from pathlib import Path
import math,struct,zlib,wave,hashlib
ROOT=Path(__file__).resolve().parents[1]
OUT=ROOT/'game/web/assets'
IMAGES=['intro','main_menu','game_over','mission_clear','apk_icon']
AUDIO=['beam','bullet','explosion','game_over','heal','mission_clear','player_hit','shoot','start']
def png(path,w,h,seed):
    rows=[]
    for y in range(h):
        line=bytearray([0])
        for x in range(w):
            dx=(x-w/2)/(w/2);dy=(y-h/2)/(h/2)
            ring=abs(math.hypot(dx,dy)-.57)<.018
            spark=((x*17+y*31+seed*101)%223)<2
            a=max(0,1-math.hypot(dx,dy))
            line.extend((int(5+23*a+45*ring),int(15+70*a+85*ring+50*spark),min(255,int(35+100*a+90*ring+80*spark)),255))
        rows.append(bytes(line))
    raw=zlib.compress(b''.join(rows),9)
    def chunk(tag,data):return struct.pack('>I',len(data))+tag+data+struct.pack('>I',zlib.crc32(tag+data)&0xffffffff)
    path.write_bytes(b'\x89PNG\r\n\x1a\n'+chunk(b'IHDR',struct.pack('>IIBBBBB',w,h,8,6,0,0,0))+chunk(b'IDAT',raw)+chunk(b'IEND',b''))
def wav(path,seed):
    rate=11025;length=.14+seed*.035
    samples=bytearray()
    for i in range(int(rate*length)):
        t=i/rate;env=(1-t/length)**2
        f=220+seed*47+120*t/length
        v=math.sin(2*math.pi*f*t)*env*.32+math.sin(2*math.pi*(f*1.5)*t)*env*.12
        samples+=struct.pack('<h',int(max(-1,min(1,v))*32767))
    with wave.open(str(path),'wb') as f:f.setnchannels(1);f.setsampwidth(2);f.setframerate(rate);f.writeframes(samples)
def main():
    (OUT/'images').mkdir(parents=True,exist_ok=True)
    (OUT/'audio').mkdir(parents=True,exist_ok=True)
    for i,name in enumerate(IMAGES):
        p=OUT/'images'/f'{name}.png'
        if not p.exists():png(p,256 if name=='apk_icon' else 360,256 if name=='apk_icon' else 640,i+1)
    for i,name in enumerate(AUDIO):
        p=OUT/'audio'/f'{name}.wav'
        if not p.exists():wav(p,i+1)
    dest=ROOT/'android/app/src/main/assets/game/assets'
    for p in OUT.rglob('*'):
        if p.is_file():
            target=dest/p.relative_to(OUT);target.parent.mkdir(parents=True,exist_ok=True)
            if not target.exists():target.write_bytes(p.read_bytes())
    errors=[]
    for p in OUT.rglob('*'):
        if p.is_file():
            target=dest/p.relative_to(OUT)
            if hashlib.sha256(p.read_bytes()).digest()!=hashlib.sha256(target.read_bytes()).digest():errors.append(str(p))
    if errors:raise SystemExit('Asset mismatch: '+str(errors))
    print('ASSET_GENERATOR PASS images=5 audio=9 synchronized=14 source=existing-or-procedural-fallback')
if __name__=='__main__':main()
