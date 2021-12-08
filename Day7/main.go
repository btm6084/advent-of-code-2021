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

	crabs := slice.StringToInt(strings.Split(string(raw), ","))
	cc, cp, ec, ep := CrabWalk(crabs)

	fmt.Printf("Part 1: Move all crabs to %d at cost %d\n", cp, cc)
	fmt.Printf("Part 2: Move all crabs to %d at cost %d\n", ep, ec)
}

func CrabWalk(crabs []int) (int, int, int, int) {
	max := maxInt(crabs)
	min := minInt(crabs)
	cheap := make([]int, max-min)
	exp := make([]int, max-min)

	for i := min; i < max; i++ {
		for c := 0; c < len(crabs); c++ {
			step := abs(crabs[c] - i)
			cheap[i] += step
			exp[i] += (step * (1 + step)) / 2
		}
	}

	cc, cp, ec, ep := 0, -1, 0, -1
	for i := 0; i < len(crabs); i++ {
		if cp < 0 || cheap[i] < cc {
			cc = cheap[i]
			cp = i
		}

		if ep < 0 || exp[i] < ec {
			ec = exp[i]
			ep = i
		}
	}

	return cc, cp, ec, ep
}

func abs(in int) int {
	if in < 0 {
		return in * -1
	}

	return in
}

func maxInt(search []int) int {
	max := 0
	for i := 0; i < len(search); i++ {
		if max < search[i] {
			max = search[i]
		}
	}

	return max
}

func minInt(search []int) int {
	if len(search) == 0 {
		return 0
	}

	if len(search) == 1 {
		return search[0]
	}

	min := search[0]

	for i := 0; i < len(search); i++ {
		if min > search[i] {
			min = search[i]
		}
	}

	return min
}
