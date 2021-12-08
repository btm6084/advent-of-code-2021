package main

import (
	"log"
	"os"
	"strings"
	"testing"

	"github.com/btm6084/utilities/slice"
)

var crabs []int

func init() {
	raw, err := os.ReadFile("./input.real.txt")
	if err != nil {
		log.Fatal(err)
	}

	crabs = slice.StringToInt(strings.Split(string(raw), ","))
}

func BenchmarkRun(b *testing.B) {
	for i := 0; i < b.N; i++ {
		CrabWalk(crabs)
	}
}
