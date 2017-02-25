#!/bin/bash
TEST_SCRIPT_PATH="./runTest.sh"

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
    printf "Usage: $0 {full|simple|example}\n"
    exit 1
esac

# Run tests
printf "Running ${testcases[*]} tests.\n"
for testcase in ${testcases[*]}
do
  bash $TEST_SCRIPT_PATH $testcase &
done

wait