from pathlib import Path
import hashlib,sys
root=Path(__file__).resolve().parents[1]
source=root/'game/web'
target=root/'android/app/src/main/assets/game'
files=sorted(p.relative_to(source) for p in source.rglob('*') if p.is_file())
missing=[];different=[]
for f in files:
 a=source/f;b=target/f
 if not b.is_file():missing.append(str(f))
 elif hashlib.sha256(a.read_bytes()).digest()!=hashlib.sha256(b.read_bytes()).digest():different.append(str(f))
extra=sorted(str(p.relative_to(target)) for p in target.rglob('*') if p.is_file() and p.relative_to(target) not in files)
print('assets:',len(files),'missing:',missing,'different:',different,'extra:',extra)
if missing or different or extra:sys.exit(1)
