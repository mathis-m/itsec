#!/bin/bash

for i in {1..500000}
do
   echo -n "p a 3bc" | zzuf -s $i -c -r 0.2 > input.txt

  ./fuzz < input.txt > /dev/null
   if [ $? -ne 0 ] ; then
	 echo "--------------------------------------------"
	 echo "Error for seed $i:" 
	 echo "Problematic input (unencoded!):"
	 cat input.txt
	 echo " - input also saved to input.txt"
	 echo "--------------------------------------------"
  	 
	 ./fuzz < input.txt 
	 exit 0 
   fi
done
