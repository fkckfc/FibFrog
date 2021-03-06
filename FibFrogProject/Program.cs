﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FibFrogProject
{
    class Program
    {
        static void Main(string[] args)
        {
            SolutionOriginal sol = new SolutionOriginal();
            //SolutionBreadthFirstSearch sol = new SolutionBreadthFirstSearch();
            int[] arg = GetRandomOne(100000); //{ 1, 1, 0, 0, 0 };

            var watch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("answer : " +sol.solution(arg));
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("time : " + elapsedMs.ToString());

            Console.ReadKey();
        }

        static int[] GetRandomOne(int length)
        {
            List<int> route = new List<int>();
            Random rnd = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < length; i++)
            {
                if (rnd.Next(0, 3) > 0)
                    route.Add(1);
                else
                    route.Add(0);
            }

            return route.ToArray();
        }
    }    

    #region SolutionOriginal
    class SolutionOriginal //91%
    {
        int[] leavesArray;
        List<int> fibonacci;
        List<int> completedJumpList = new List<int>();
        //to improve performance by skipping subsequent jump larger than this
        int shortestJump;

        public int solution(int[] A)
        {
            // write your code in C# 6.0 with .NET 4.5 (Mono)

            //create Fibonacci list
            leavesArray = A;
            fibonacci = CreateFibonacci(A.Length);

            shortestJump = A.Length + 2;
            RecursiveJump(0, 0);

            if (completedJumpList.Count > 0)
                return completedJumpList.Min();
            else
                return -1;


        }

        private void RecursiveJump(int currentLocation, int numOfJump)
        {
            if (numOfJump >= shortestJump) return;

            if (currentLocation >= leavesArray.Length)  //ady moved more than half way, start from beginning
            {
                for (int fibIndex = 0; fibIndex < fibonacci.Count; fibIndex++)
                {
                    if (numOfJump + 1 >= shortestJump) break;

                    if (JumpLogic(currentLocation, numOfJump, fibIndex))
                        break;
                }
            }
            else //moved less than half way, use big fib num
            {
                for (int fibIndex = (fibonacci.Count - 1); fibIndex >= 0; fibIndex--)
                {
                    if (numOfJump + 1 >= shortestJump) break;

                    JumpLogic(currentLocation, numOfJump, fibIndex);
                }
            }
        }

        private bool JumpLogic(int currentLocation, int numOfJump, int fibIndex)
        {
            bool isOver = false;
            int nextLocation = currentLocation + fibonacci[fibIndex];

            if (nextLocation > leavesArray.Length + 1) //Over jump
            {
                //Console.WriteLine("Over jump");
                isOver = true;
            }
            else if (nextLocation == leavesArray.Length + 1) //reach destination
            {
                //Console.WriteLine("reach destination");
                completedJumpList.Add(numOfJump + 1);
                if (shortestJump > numOfJump + 1)
                {
                    shortestJump = numOfJump + 1;
                    isOver = true;
                }
            }
            else if (leavesArray[nextLocation - 1] == 1)//can jump
            {
                //Console.WriteLine("can jump");
                currentLocation += fibonacci[fibIndex];
                numOfJump++;

                RecursiveJump(currentLocation, numOfJump);
                numOfJump--;
                currentLocation -= fibonacci[fibIndex];
            }
            else
            {
                //Console.WriteLine("cannot jump");
            }

            return isOver;
        }

        private List<int> CreateFibonacci(int length)
        {
            List<int> fibonacci = new List<int>();
            fibonacci.Add(1);

            if (length > 0)
                fibonacci.Add(2);

            int i = 2;
            while (i <= length)
            {
                int newFib = fibonacci[i - 1] + fibonacci[i - 2];
                if (newFib > length + 1)
                    break;
                else
                    fibonacci.Add(newFib);

                i++;
            }
            return fibonacci;
        }
    }
    #endregion    

    #region Java
    class SolutionBreadthFirstSearch
    {
	internal class Jump
        {
            public int position;
            public int numOfJumps;
            public Jump(int pos, int num)
            {
                this.position = pos;
                this.numOfJumps = num;
            }
        }

        public List<int> getFibonaci(int max)
        {
            List<int> fibs = new List<int>();
            fibs.Add(1);
            fibs.Add(1);
            int f = 1;
            while (fibs[f] <= max)
            {
                fibs.Add(fibs[f] + fibs[f - 1]);
                f++;
            }
            fibs.Remove(0);
            fibs.Reverse();
            return fibs;
        }
	    
        public int solution(int[] A) 
        {
           List<int> fibs = getFibonaci(A.Length);

            bool[] accessed = new bool[A.Length];
            Queue<Jump> queue = new Queue<Jump>();
            queue.Enqueue(new Jump(-1, 0));
            while (queue.Count > 0)
            {
                Jump currentAttempt = queue.Dequeue();
                int numOfJumps = currentAttempt.numOfJumps + 1;

                foreach (int jumpLength in fibs)
                {
                    int nextPosition = currentAttempt.position + jumpLength;
                    if (nextPosition == A.Length) //reached
                    {
                        return numOfJumps;
                    }
                    else if (nextPosition > A.Length || A[nextPosition] == 0 || accessed[nextPosition]) continue;

                    queue.Enqueue(new Jump(nextPosition, numOfJumps));
                    accessed[nextPosition] = true;
                }
            }

            return -1;
        }        
    }
    #endregion
}
