package main

import "fmt"

const (
	StateStart  = "start"
	StateA      = "A"
	StateB      = "B"
	StateC      = "C"
	StateFinish = "finish"
	StateError  = "error"
)

func isIdentifier(content string) bool {
	state := StateStart

	for i := 0; i < len(content); i++ {
		ch := content[i]

		switch state {
		case StateStart:
			if ch >= 'a' && ch <= 'z' {
				state = StateA
			} else {
				return false
			}
		case StateA:
			if ch >= 'a' && ch <= 'z' {
				state = StateA // keep
			} else if ch >= '0' && ch <= '9' {
				state = StateB
			} else {
				return false
			}
		case StateB:
			if ch >= 'a' && ch <= 'z' {
				state = StateA
			} else if ch >= '0' && ch <= '9' {
				state = StateB // keep
			} else {
				return false
			}
		}
	}

	return state == StateA || state == StateB
}

func test(word string) {
	fmt.Printf("%s => %t\n", word, isIdentifier(word))
}

func main() {
	test("h2ello")
}
