#!/usr/bin/env python3
"""Verify fallback resources are decodable and bundled, with no third-party packages."""
from pathlib import Path
import hashlib, struct, wave, sys
root=Path(__file__).resolve().parents[1]
a=root/'game/web/assets'
b=root/'android/app/src/main/assets/game/assets'
images=['intro','main_menu','game_over','mission_clear','apk_icon']
sounds=['beam','bullet','explosion','game_over','heal','mission_clear','player_hit','shoot','start']
errors=[]
for name in images:
    rel=Path('images')/(name+'.png')
    for base in (a,b):
        p=base/rel
        try:
            data=p.read_bytes()
            if data[:8]!=b'\x89PNG\r\n\x1a\n' or data[12:16]!=b'IHDR':raise ValueError('bad PNG header')
            w,h=struct.unpack('>II',data[16:24])
            if w<64 or h<64:raise ValueError('image too small')
        except Exception as e:errors.append(str(p)+': '+str(e))
for name in sounds:
    rel=Path('audio')/(name+'.wav')
    for base in (a,b):
        p=base/rel
        try:
            with wave.open(str(p),'rb') as f:
                if f.getnframes()<100 or f.getframerate()<8000:raise ValueError('bad WAV audio')
        except Exception as e:errors.append(str(p)+': '+str(e))
for rel in [*(Path('images')/(n+'.png') for n in images),*(Path('audio')/(n+'.wav') for n in sounds)]:
    try:
        if hashlib.sha256((a/rel).read_bytes()).digest()!=hashlib.sha256((b/rel).read_bytes()).digest():errors.append('bundle mismatch '+str(rel))
    except Exception as e:errors.append('compare '+str(rel)+': '+str(e))
print('RESOURCE_VALIDATION', 'PASS' if not errors else 'FAIL', 'files=',28,'issues=',len(errors))
for e in errors:print(e)
sys.exit(bool(errors))
