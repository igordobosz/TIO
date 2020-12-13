import math

class Thief():
    def __init__(self, capasity_of_knapsack, min_speed, max_speed):
        self.capasity_of_knapsack = capasity_of_knapsack
        self.min_speed = min_speed
        self.max_speed = max_speed
        self.visited_cities = []
        self.items_in_knapsack = []
        self.fitness = 0  # zysk - koszta(czas, ktory potrzebowalismy)

    def __str__(self):
        cities_seq = ""
        for city in self.visited_cities:
            cities_seq += str(city.index) + " "
        items_seq = ""
        for item in self.items_in_knapsack:
            items_seq += str(item.index) + " "
        return "Thief: [cities_seq: {}, items_seq: {}, fitness: {}]".format(cities_seq, items_seq, self.fitness)

    ##TODO: sufit z czasu podrozy
    def calculateFitness(self):
        sumOfProfit = 0
        sumOfTravel = 0
        sumOfWeight = 0
        dimensions = self.visited_cities.__len__()
        for i in range(dimensions):
            sumOfWeight += self.items_in_knapsack[i].weight
            sumOfProfit += self.items_in_knapsack[i].profit
            temp_velocity = self.max_speed - sumOfWeight*(self.max_speed-self.min_speed)/self.capasity_of_knapsack
            curr_city = self.visited_cities[i]
            if i+1 < dimensions:
                next_city = self.visited_cities[i+1]
            else:
                next_city = self.visited_cities[0]
            sumOfTravel += math.ceil(curr_city.getDistanceBtw(next_city)/temp_velocity)
        self.fitness =  sumOfTravel - sumOfProfit

