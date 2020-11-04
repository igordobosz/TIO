import random
from entities.Thief import Thief
import helpers

def selection(thieves, tour, type):
    if type =="roulette":
        return selectionRoulette(thieves)
    else:
        return selectionTour(thieves, tour)

def crossover(thieves, px, type):
    if type == "1p":
        return crossover1p(thieves, px)
    elif type == "2p":
        return crossover2p(thieves, px)
    elif type == "and":
        return crossoverand(thieves, px)

def mutation(thieves, pm, mutation):
    if mutation == "normal":
        return mutationNormal(thieves, pm)
    else:
        return mutationInversion(thieves, pm)


def selectionRoulette(thieves):
    ans = []
    percents = []
    sum = 0
    thieves.sort(key=lambda  thief: thief.fitness)
    normalize_fitness = abs(thieves[0].fitness) * 1.1
    for thief in thieves:
        sum += thief.fitness + normalize_fitness
    for thief in thieves:
        percents.append((thief.fitness + normalize_fitness)/sum)
    numOfThieves = thieves.__len__()
    for index in range(numOfThieves):
        randInt = random.randrange(0,numOfThieves)/(numOfThieves)
        tmp_sum = 0
        for j in range(percents.__len__()):
            if (randInt < tmp_sum + percents[j]):
                ans.append(thieves[j])
                break
            else:
                tmp_sum+=percents[j]
    return ans

def selectionTour(thieves, tour):
    ans = []
    if tour==1:
        return thieves
    elif tour == thieves.__len__():
        thieves.sort(key=lambda  thief: thief.fitness, reverse=True)
        for i in range (thieves.__len__()):
            ans.append(thieves[0])
        return ans
    for i in range(thieves.__len__()):
        tour_arr = []
        for i in range (tour):
            rand = random.randrange(0, thieves.__len__())
            tour_arr.append(thieves[rand])
        tour_arr.sort(key=lambda  thief: thief.fitness, reverse=True)
        ans.append(tour_arr[0])
    return ans

def crossover2p(thieves, px):
    ans = randomThievesList(thieves)
    numOfThieves = ans.__len__()
    for pair in range(0, numOfThieves, 2):
        thief_1 = ans[pair]
        thief_2 = ans[pair+1]

        if random.randrange(100)/100 < px :
            locuses = [random.randrange(thief_1.visited_cities.__len__()) for _ in range(2)]
            locuses.sort()

            thief_n1 = Thief(thief_1.capasity_of_knapsack, thief_1.min_speed, thief_1.max_speed)
            thief_n1.visited_cities = thief_1.visited_cities[:locuses[0]] + thief_2.visited_cities[locuses[0]:locuses[1]] + thief_1.visited_cities[locuses[1]:]
            repairThief(thief_n1, thief_1.visited_cities[:])
            thief_n1.items_in_knapsack = helpers.getListOfItemsBasedOnCities(thief_n1.visited_cities, thief_n1.capasity_of_knapsack)

            thief_n1.calculateFitness()

            thief_n2 = Thief(thief_1.capasity_of_knapsack, thief_1.min_speed, thief_1.max_speed)
            thief_n2.visited_cities = thief_2.visited_cities[:locuses[0]] + thief_1.visited_cities[locuses[0]:locuses[1]] + thief_2.visited_cities[locuses[1]:]
            repairThief(thief_n2, thief_1.visited_cities[:])
            thief_n2.items_in_knapsack = helpers.getListOfItemsBasedOnCities(thief_n2.visited_cities, thief_n2.capasity_of_knapsack)
            thief_n2.calculateFitness()

            ans[pair] = thief_n1
            ans[pair+1] = thief_n2
    return ans


def crossoverand(thieves, px):
    ans = randomThievesList(thieves)
    numOfThieves = ans.__len__()
    for pair in range(0, numOfThieves, 2):
        thief_1 = ans[pair]
        thief_2 = ans[pair+1]

        if random.randrange(100)/100 < px :
            locus = random.randrange(thief_1.visited_cities.__len__())

            thief_n1 = Thief(thief_1.capasity_of_knapsack, thief_1.min_speed, thief_1.max_speed)
            for index in range(thief_1.visited_cities.__len__()):
                if thief_1.visited_cities[index].index > locus:
                    thief_n1.visited_cities.append(thief_1.visited_cities[index])
                else:
                    thief_n1.visited_cities.append(thief_2.visited_cities[index])
            repairThief(thief_n1, thief_1.visited_cities[:])
            thief_n1.items_in_knapsack = helpers.getListOfItemsBasedOnCities(thief_n1.visited_cities, thief_n1.capasity_of_knapsack)

            thief_n1.calculateFitness()

            thief_n2 = Thief(thief_1.capasity_of_knapsack, thief_1.min_speed, thief_1.max_speed)
            for index in range(thief_1.visited_cities.__len__()):
                if thief_1.visited_cities[index].index > locus:
                    thief_n2.visited_cities.append(thief_1.visited_cities[index])
                else:
                    thief_n2.visited_cities.append(thief_2.visited_cities[index])
            repairThief(thief_n2, thief_1.visited_cities[:])
            thief_n2.items_in_knapsack = helpers.getListOfItemsBasedOnCities(thief_n2.visited_cities, thief_n2.capasity_of_knapsack)
            thief_n2.calculateFitness()

            ans[pair] = thief_n1
            ans[pair+1] = thief_n2
    return ans


def crossover1p(thieves, px):
    ans = randomThievesList(thieves)
    numOfThieves = ans.__len__()
    for pair in range(0, numOfThieves, 2):
        thief_1 = ans[pair]
        thief_2 = ans[pair+1]

        if random.randrange(100)/100 < px :
            locus = random.randrange(thief_1.visited_cities.__len__())

            thief_n1 = Thief(thief_1.capasity_of_knapsack, thief_1.min_speed, thief_1.max_speed)
            thief_n1.visited_cities = thief_1.visited_cities[:locus] + thief_2.visited_cities[locus:]
            repairThief(thief_n1, thief_1.visited_cities[:])
            thief_n1.items_in_knapsack = helpers.getListOfItemsBasedOnCities(thief_n1.visited_cities, thief_n1.capasity_of_knapsack)

            thief_n1.calculateFitness()

            thief_n2 = Thief(thief_1.capasity_of_knapsack, thief_1.min_speed, thief_1.max_speed)
            thief_n2.visited_cities = thief_2.visited_cities[:locus] + thief_n1.visited_cities[locus:]
            repairThief(thief_n2, thief_1.visited_cities[:])
            thief_n2.items_in_knapsack = helpers.getListOfItemsBasedOnCities(thief_n2.visited_cities, thief_n2.capasity_of_knapsack)
            thief_n2.calculateFitness()

            ans[pair] = thief_n1
            ans[pair+1] = thief_n2
    return ans

def randomThievesList(thieves):
    ans = []
    while thieves.__len__()>0:
        rand = random.randrange(0, thieves.__len__())
        ans.append(thieves[rand])
        del thieves[rand]
    return ans


def repairThief(thief_n1, cities):
    cities_dic = {}
    for t_city in thief_n1.visited_cities:
        check = False
        for c_city in cities:
            if c_city == t_city:
                check = True
                break
        if check:
            cities.remove(t_city)
        else:
            if cities_dic.get(t_city) != None:
                cities_dic[t_city] += 1
            else:
                cities_dic[t_city] = 1
    for index in range(thief_n1.visited_cities.__len__()):
        if cities_dic.__len__() == 0 and cities.__len__ == 0:
            break
        tmp_city = thief_n1.visited_cities[index]
        if cities_dic.get(tmp_city) != None:
            cities_dic[tmp_city] -= 1
            if cities_dic[tmp_city] == 0:
                cities_dic.pop(tmp_city)
            thief_n1.visited_cities[index] = cities[0]
            del cities[0]

def mutationNormal(thieves, pm):
    for thief in thieves:
        if random.randrange(100)/100 < pm:
            rand_1 = random.randrange(thief.visited_cities.__len__())
            rand_2 = random.randrange(thief.visited_cities.__len__())
            if rand_1 != rand_2:
                tmp_c1 = thief.visited_cities[rand_1]
                tmp_c2 = thief.visited_cities[rand_2]

                thief.visited_cities[rand_1] = tmp_c2
                thief.visited_cities[rand_2] = tmp_c1
    return thieves

def mutationInversion(thieves, pm):
    for thief in thieves:
        if random.randrange(100)/100 < pm:
            locuses = [random.randrange(thief.visited_cities.__len__()) for _ in range(2)]
            locuses.sort()
            thief.visited_cities = thief.visited_cities[:locuses[0]] + list(reversed(thief.visited_cities[locuses[0]:locuses[
                1]])) + thief.visited_cities[locuses[1]:]

    return thieves