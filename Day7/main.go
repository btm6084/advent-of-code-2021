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
	cheap := make([]int, len(crabs))
	exp := make([]int, len(crabs))

	for i := 0; i < len(crabs); i++ {
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

	fmt.Printf("Part 1: Move all crabs to %d at cost %d\n", cp, cc)
	fmt.Printf("Part 2: Move all crabs to %d at cost %d\n", ep, ec)

}

func abs(in int) int {
	if in < 0 {
		return in * -1
	}

	return in
}
