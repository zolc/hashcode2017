#!/bin/bash
TEST_SCRIPT_PATH="./runTest.sh"
PARALLEL_TESTS=0

if [ $# -eq 0 ]; then
  printf "Usage: $0 {full|simple|example} [-p]\n"
  printf "The -p flag is for running each task simultaneously\n"
  exit 1
fi

# Prepare test suite
TEST_SUITE="$1"
case "$TEST_SUITE" in
  full)
    testcases=(me_at_the_zoo trending_today videos_worth_spreading kittens)
    ;;

  simple)
    testcases=(me_at_the_zoo videos_worth_spreading)
    ;;

  example)
    testcases=(me_at_the_zoo)
    ;;
  
  *)
    printf "Usage: $0 {full|simple|example} [-p]\n"
    printf "The -p flag is for running each task simultaneously\n"
    exit 1
esac

# Check if tests should be run in parallel
if [ $# -eq 2 ]; then
  if [ "$2" == "-p" ]; then
    PARALLEL_TESTS=1
  fi
fi

# Run tests
printf "Running ${testcases[*]} tests "
if [ $PARALLEL_TESTS -eq 1 ]; then
  printf "in parallel (multiple processes)"
else
  printf "synchronously (one after another)"
fi
printf "\n"

for testcase in ${testcases[*]}
do
  if [ $PARALLEL_TESTS -eq 1 ]; then
    bash $TEST_SCRIPT_PATH $testcase &
  else
    bash $TEST_SCRIPT_PATH $testcase
  fi
done

wait
