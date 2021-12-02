package main

import (
	"fmt"
	"log"
	"os"
	"strings"

	"github.com/btm6084/utilities/slice"
	"github.com/spf13/cast"
)

func main() {
	raw, err := os.ReadFile("./input.txt")
	if err != nil {
		log.Println(err)
		return
	}

	pieces := strings.Split(string(raw), "\n")
	input := make([]int, len(pieces))

	for k, v := range pieces {
		input[k] = cast.ToInt(v)
	}

	Part1(input)
	Part2(input)
}

func Part1(input []int) {
	prev := -1
	count := 0
	for _, v := range input {
		if prev > 0 && v > prev {
			count++
		}
		prev = v
	}

	fmt.Printf("Part 1: There were %d depth increases\n", count)
}

func Part2(input []int) {
	prev := -1
	count := 0

	for i := 0; i < len(input)-2; i++ {
		sum := slice.SumInts(input[i : i+3])
		if prev > 0 && sum > prev {
			count++
		}

		prev = sum
	}

	fmt.Printf("Part 2: There were %d depth increases\n", count)
}
