#!/bin/bash

for i in {1..500000}
do
    while read -r line
    do
        payload=$(echo $line | zzuf -s $i -c -r 0.2)
        echo "h $payload" > input.txt
        
        ./fuzz < input.txt > /dev/null
        if [ $? -ne 0 ] ; then
            echo "--------------------------------------------"
            echo "Normal Input:"
            echo $line
            echo "--------------------------------------------"
            echo "Hex Error for seed $i:"
            echo "Problematic input (unencoded!):"
            cat input.txt
            echo " - input also saved to input.txt"
            echo "--------------------------------------------"
            
            ./fuzz < input.txt
            exit 0
        fi

        echo "l $payload" > input.txt
        
        ./fuzz < input.txt > /dev/null
        if [ $? -ne 0 ] ; then
            echo "--------------------------------------------"
            echo "Normal Input:"
            echo $line
            echo "--------------------------------------------"
            echo "toLower Error for seed $i:"
            echo "Problematic input (unencoded!):"
            cat input.txt
            echo " - input also saved to input.txt"
            echo "--------------------------------------------"
            
            ./fuzz < input.txt
            exit 0
        fi
    done < "./example2.txt"
done