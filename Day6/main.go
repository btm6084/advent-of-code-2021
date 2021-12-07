package main

import (
	"fmt"
	"log"
	"os"
	"strings"

	"github.com/btm6084/utilities/slice"
)

func main() {
	raw, err := os.ReadFile("./input.real.txt")
	if err != nil {
		log.Fatal(err)
	}

	input := slice.StringToInt(strings.Split(string(raw), ","))
	fish := make([]int, 9)

	for _, c := range input {
		fish[c]++
	}

	for day := 0; day < 256; day++ {
		zeros := fish[0]
		for i := 1; i < len(fish); i++ {
			if fish[i] == 0 {
				continue
			}

			fish[i-1] += fish[i]
			fish[i] -= fish[i]
		}
		fish[6] += zeros
		fish[8] += zeros
		fish[0] -= zeros

		if day == 79 {
			fmt.Printf("Part 1: %d fish\n", slice.SumInts(fish))
		}
	}

	fmt.Printf("Part 2: %d fish\n", slice.SumInts(fish))

}
