#!/bin/sh

for it in $(find . -name "*.xxd"); do
  cat "${it}" | sort | uniq > "${it}.tmp"
  mv "${it}.tmp" "${it}"
done

