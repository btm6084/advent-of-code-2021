package main

import (
	"fmt"
	"log"
	"os"
	"strings"

	"github.com/spf13/cast"
)

func main() {
	raw, err := os.ReadFile("./input.real.txt")
	if err != nil {
		log.Fatal(err)
	}

	lines := strings.Split(string(raw), "\n")
	Part1(lines)
	Part2(lines)

}

func Part1(lines []string) {
	sum := make([]int, len(lines[0]))
	check := len(lines) / 2

	for _, l := range lines {
		for k, b := range l {
			if b == '1' {
				sum[k]++
			}
		}
	}

	gamma := 0
	epsilon := 0
	pos := len(sum) - 1
	for i := pos; i >= 0; i-- {
		magnitude := pow(2, (pos - i))
		if sum[i] > check {
			gamma += magnitude
		} else {
			epsilon += magnitude
		}
	}

	fmt.Printf("Part 1: Gamma: %d Epsilon: %d Product: %d\n", gamma, epsilon, gamma*epsilon)
}

func Part2(lines []string) {
	i := 0
	oxyLines := make([]string, len(lines))
	copy(oxyLines, lines)
	for len(oxyLines) > 1 {
		oxyLines = filter(oxyLines, i, false)
		i++
	}

	oxy := binToInt(oxyLines[0])

	i = 0
	c02Lines := make([]string, len(lines))
	copy(c02Lines, lines)
	for len(c02Lines) > 1 {
		c02Lines = filter(c02Lines, i, true)
		i++
	}

	c02 := binToInt(c02Lines[0])

	fmt.Println("Part 2:", "Oxygen:", oxy, "C02:", c02, "Product:", oxy*c02)
}

func filter(in []string, pos int, lowest bool) []string {
	ones, zeros := 0, 0
	for _, l := range in {
		if l[pos] == '1' {
			ones++
		} else {
			zeros++
		}
	}

	var bit byte = '1'
	if (lowest && ones >= zeros) || (!lowest && ones < zeros) {
		bit = '0'
	}

	count := 0
	for i := 0; i < len(in); i++ {
		if in[i][pos] == bit {
			in[count] = in[i]
			count++
		}
	}

	return in[:count]
}

func pow(a, b int) int {
	out := a
	if b == 0 {
		return 1
	}

	for i := 1; i < b; i++ {
		out *= a
	}

	return out
}

func binToInt(b string) int {
	sum := 0
	pos := len(b) - 1

	for i := pos; i >= 0; i-- {
		mag := pow(2, (pos - i))
		sum += cast.ToInt(string(b[i])) * mag
	}

	return sum
}
