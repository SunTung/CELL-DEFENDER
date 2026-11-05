#!/usr/bin/env python3
"""Fail closed if Stage 15 binary assets are missing or modified."""
from pathlib import Path
import hashlib,sys
root=Path(__file__).resolve().parents[1]
lines=(root/'scripts/stage15_assets.sha256').read_text().splitlines()
errors=[]
for line in lines:
    rel,size,sha=line.split()
    for base in ('game/web/assets','android/app/src/main/assets/game/assets'):
        p=root/base/rel
        if not p.is_file():errors.append('MISSING '+str(p.relative_to(root)));continue
        if p.stat().st_size!=int(size) or hashlib.sha256(p.read_bytes()).hexdigest()!=sha:
            errors.append('MISMATCH '+str(p.relative_to(root)))
print('STAGE15_BINARY_PREFLIGHT', 'PASS' if not errors else 'FAIL', 'expected=',len(lines)*2,'errors=',len(errors))
for e in errors:print(e)
sys.exit(bool(errors))
