package main

import (
	"fmt"

	log "github.com/sirupsen/logrus"
	"github.com/spf13/cast"

	"os"
	"regexp"
	"strings"
)

var (
	instRE = regexp.MustCompile(`^(forward|up|down) ([0-9]+)$`)
)

func input() ([]string, error) {
	raw, err := os.ReadFile("./input.real.txt")
	if err != nil {
		return nil, err
	}

	return strings.Split(string(raw), "\n"), nil
}

func main() {
	input, err := input()
	if err != nil {
		log.Println(err)
		return
	}

	Part1(input)
	Part2(input)
}

func Part1(input []string) {
	h := 0
	d := 0

	for _, inst := range input {
		matches := instRE.FindAllStringSubmatch(inst, -1)

		if len(matches) == 0 {
			log.Fatalf("No matches for instruction: %s", inst)
		}

		val := cast.ToInt(matches[0][2])

		switch matches[0][1] {
		case "forward":
			h += val
		case "down":
			d += val
		case "up":
			d -= val
		default:
			log.Fatalf("Invalid instruction: %s", inst)
		}
	}

	fmt.Printf("Part 1: Position: [%d,%d] Value: %d\n", h, d, h*d)
}

func Part2(input []string) {
	h := 0
	d := 0
	a := 0

	for _, inst := range input {
		matches := instRE.FindAllStringSubmatch(inst, -1)

		if len(matches) == 0 {
			log.Fatalf("No matches for instruction: %s", inst)
		}

		val := cast.ToInt(matches[0][2])

		switch matches[0][1] {
		case "forward":
			h += val
			d += val * a
		case "down":
			a += val
		case "up":
			a -= val
		default:
			log.Fatalf("Invalid instruction: %s", inst)
		}
	}

	fmt.Printf("Part 1: Position: [%d,%d] Value: %d\n", h, d, h*d)
}
