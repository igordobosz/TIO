from entities.City import City
from entities.Item import Item
import random

def loader(file_name):
    f = open(file_name, "r")
    contents = f.readlines()
    temp = contents[2].split("\t")
    dimensions = int(temp[temp.__len__() - 1])

    temp = contents[3].split("\t")
    number_of_items = int(temp[temp.__len__() - 1])

    temp = contents[4].split("\t")
    capacity_of_knapsack = int(temp[temp.__len__() - 1])

    temp = contents[5].split("\t")
    min_speed = float(temp[temp.__len__() - 1])

    temp = contents[6].split("\t")
    max_speed = float(temp[temp.__len__() - 1])

    cities = []

    for i in range(dimensions):
        temp = contents[10 + i].split("\t")
        index = int(temp[0])
        cord_x = float(temp[1])
        cord_y = float(temp[2])

        city = City(index, cord_x, cord_y)
        cities.append(city)

    for i in range(number_of_items):
        temp = contents[11 + dimensions + i].split("\t")
        index = int(temp[0])
        reward = int(temp[1])
        weight = int(temp[2])
        city_index = int(temp[3])

        item = Item(index, reward, weight)

        for city_object in cities:
            if city_object.index == city_index:
                city_object.addItem(item)

    return cities, capacity_of_knapsack, min_speed, max_speed

def generateRandomCityTravelSeq(cities):
    ans = []
    while cities.__len__() > 0:
        rand_index = random.randint(0, cities.__len__()-1)
        ans.append(cities[rand_index])
        del cities[rand_index]
    return ans

def getListOfItemsBasedOnCities(cities, capacity_of_knapsack):
    ans = []
    sumWeight = 0
    for city in cities:
        if city.items.__len__()>0:
            city.items.sort(key = lambda item: item.profit, reverse=True)
            for item in city.items:
                if (sumWeight + item.weight) <= capacity_of_knapsack:
                    sumWeight += item.weight
                    ans.append(item)
                    break
            ans.append(Item(-1, 0, 0))
        else:
            ans.append(Item(-1, 0 , 0))
    return ans
