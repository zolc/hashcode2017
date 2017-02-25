#!/bin/bash

PROGRAM_PATH="./Problem/bin/Debug/Problem.exe"

if [ ! -f "$PROGRAM_PATH" ]; then
  printf "Program is not built yet. Please built it first or change the PROGRAM_PATH variable if\n"
  printf "the problem persists.\n"
  printf "Current PROGRAM_PATH: $PROGRAM_PATH"
  exit 1
fi

TESTCASE="$1"
INPUT_FILE="$TESTCASE.in"
OUTPUT_FILE="$TESTCASE.out"
printf "Executing the program for the following test case: $INPUT_FILE\n"

if [ ! -f "$INPUT_FILE" ]; then
  printf "Test case does not exist.\n";
  exit 2
fi

"$PROGRAM_PATH" < "$INPUT_FILE" > "$OUTPUT_FILE"
printf "Test case $TESTCASE finished. Output saved in $OUTPUT_FILE\n"
