#!/bin/sh

for it in $(find . -name "*.bin"); do
  hexdump -C    "${it}" > "${it}.hxd"
done

