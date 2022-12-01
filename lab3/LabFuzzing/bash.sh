#!/bin/bash
for i in {1..100}; do for f in example.txt; do zzuf -r 0.05 -s $i < "$f">"$i-$f"; done; done
