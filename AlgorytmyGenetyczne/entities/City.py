import math

class City():
    def __init__(self,index, cord_x, cord_y):
        self.index=index
        self.cord_x=cord_x
        self.cord_y=cord_y
        self.items=[]

    def __str__(self):
        return "City: [index = {}, cord_x = {}, cord_y = {}, items = {}]".format(self.index, self.cord_x, self.cord_y, self.items.__len__())

    def addItem(self, item):
        self.items.append(item)

    def getDistanceBtw(self, ot_city):
        return math.sqrt(math.pow(self.cord_x - ot_city.cord_x, 2) + math.pow(self.cord_y - ot_city.cord_y, 2))
