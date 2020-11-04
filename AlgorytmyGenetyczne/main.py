# import pandas as py
# import numpy as np
# import random

import time
from datetime import datetime
import helpers
from entities.Thief import Thief
from entities.Population import Population
import algorithm
from timeit import default_timer as timer
import gc

class Algorithm():
    def __init__(self, index, pop_size, gen, px, pm, tour, file_name, selection, ifWrite, crossover, mutation):
        self.index = index
        self.pop_size = pop_size
        self.gen = gen
        self.px = px
        self.pm = pm
        self.tour = tour
        self.file_name = file_name
        self.selection = selection
        self.crossover = crossover
        self.ifWrite = ifWrite
        self.populations = []
        self.cities = []
        self.capacity_of_knapsack = 0
        self.min_speed = 0
        self.max_speed = 0
        self.time = 0
        self.mutation = mutation

        self.start()

    def __str__(self):
        return "Alg: [index={}, filename={}, selection={}, crossover = {}, gen={}, time={}]".format(self.index, self.file_name, self.selection,self.crossover, self.gen, self.time)

    def getCsvString(self):
        return "{}, {}, {},{}\n".format(self.index,int(self.populations[-1].best_thief.fitness),int((self.populations[-1].best_thief.fitness-self.populations[0].best_thief.fitness)*100/self.populations[0].best_thief.fitness), int((self.populations[-1].avg-self.populations[0].avg)*100/self.populations[0].avg))

    def getBest(self):
        return self.populations[-1].best_thief.fitness

    def generateFirstPopulation(self):
        thieves = []
        for i in range(0, self.pop_size):
            thief = Thief(self.capacity_of_knapsack, self.min_speed, self.max_speed)
            thief.visited_cities = helpers.generateRandomCityTravelSeq(self.cities[:])
            thief.items_in_knapsack = helpers.getListOfItemsBasedOnCities(thief.visited_cities, self.capacity_of_knapsack)
            thief.calculateFitness()
            thieves.append(thief)
        return Population(1, thieves)

    def start(self):
        start = timer()
        self.cities, self.capacity_of_knapsack, self.min_speed, self.max_speed = helpers.loader("data/" + self.file_name + ".ttp")

        self.populations.append(self.generateFirstPopulation())
        self.populations[0].calcParameters()

        curr_pop_index = self.populations[0].index
        while curr_pop_index < self.gen:
            thieves = algorithm.selection(self.populations[curr_pop_index - 1].thievefs[:], self.tour, self.selection)
            thieves = algorithm.crossover(thieves[:], self.px, self.crossover)
            thieves = algorithm.mutation(thieves[:], self.pm, self.mutation)
            del self.populations[curr_pop_index - 1].thievefs
            print(curr_pop_index)
            curr_pop_index += 1
            new_pop = Population(curr_pop_index, thieves)
            new_pop.calcParameters()
            self.populations.append(new_pop)
            gc.collect()

        if self.ifWrite:
            date = datetime.now()
            # f = open("stats/{}_{}_{}_{}_{}.csv".format(self.file_name,self.selection ,self.gen, date.strftime("%Y-%m-%d"), date.strftime("%H-%M-%S")), "w")
            # f = open("stats/{}_{}_{}_{}_{}_{}_{}_{}.csv".format(self.file_name,self.selection ,self.gen, self.tour, self.px*100, self.pm*100, self.pop, self.gen), "w")
            f = open("stats/{}_{}_{}_{}_{}_{}_{}_{}_{}.csv".format(self.file_name,self.selection ,self.gen, self.tour, self.px*100, self.pm*100, self.pop_size, self.crossover, self.mutation), "w")
            f.write("index, best_thief, avg, worst_thief\n")
            for pop in self.populations:
                f.write(pop.getCsvString())

        end = timer()
        self.time = end - start


pop_size = 150
gen = 500
px = 0.7
pm = 0.2
tour = 5
file_name = "hard_0"
selection = "tour"
crossover = "2p"
mutation = "normal1"
ifWrite = True
iterator = 1

# if iterator>1:
#     date = datetime.now()
#     f = open("stats/{}_{}_{}_{}_{}.csv".format(file_name,se1081
# 1082
# 1083
# 1084
# 1085
# 1086
# 1087
# 1088
# 1089
# 1090
# 1091
# 1092lection,gen, tour, crossover), "w")
#     f.write("index, best_thief,best_thief%,avg%\n")
# px_arr = [0.5, 0.6, 0.7, 0.8, 0.9, 1]
# pm_arr = [0.1, 0.05, 0.01, 0.15, 0.2]
# gen_arr = [100, 500, 1000]

for i in range(iterator):
    alg = Algorithm(i, pop_size, gen, px, pm, tour, file_name, selection, ifWrite, crossover, mutation)
    if iterator>1:
        print(alg)
        # f.write(alg.getCsvString())
    else:
        print(alg)