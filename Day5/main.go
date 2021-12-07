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

	points := slice.StringToInt(strings.Split(strings.ReplaceAll(strings.ReplaceAll(string(raw), " -> ", ","), "\n", ","), ","))
	part1 := make(map[string]int)
	part2 := make(map[string]int)

	for idx := 0; idx < len(points); idx += 4 {
		x1, y1, x2, y2 := points[idx], points[idx+1], points[idx+2], points[idx+3]
		xSlope := x1 - x2
		xDir := -1
		if xSlope < 0 {
			xDir = 1
		}

		ySlope := y1 - y2
		yDir := -1
		if ySlope < 0 {
			yDir = 1
		}

		if abs(xSlope) == abs(ySlope) {
			for i := 0; i < abs(xSlope)+1; i++ {
				key := fmt.Sprintf("%d,%d", x1+(i*xDir), y1+(i*yDir))
				part2[key]++
			}
		}

		if xSlope != 0 && ySlope != 0 {
			continue
		}

		if xSlope != 0 {
			for i := 0; i < abs(xSlope)+1; i++ {
				key := fmt.Sprintf("%d,%d", x1+(i*xDir), y1)
				part1[key]++
				part2[key]++
			}
		}
		if ySlope != 0 {
			for i := 0; i < abs(ySlope)+1; i++ {
				key := fmt.Sprintf("%d,%d", x1, y1+(i*yDir))
				part1[key]++
				part2[key]++
			}
		}
	}

	count := 0
	for _, c := range part1 {
		if c > 1 {
			count++
		}
	}

	fmt.Printf("Part 1: %d overlaps\n", count)

	count = 0
	for _, c := range part2 {
		if c > 1 {
			count++
		}
	}

	fmt.Printf("Part 2: %d overlaps\n", count)
}

func abs(in int) int {
	if in < 0 {
		return in * -1
	}
	return in
}
