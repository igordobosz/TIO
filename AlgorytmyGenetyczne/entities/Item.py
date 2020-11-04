class Item():
    def __init__(self,index, profit, weight):
        self.index=index
        self.profit=profit
        self.weight=weight
        if weight > 0:
            self.profitability=profit/weight
        else:
            self.profitability = 0

    def __str__(self):
        return "Item: [index = {}, profit = {}, weight = {}, profitability = {}]".format(self.index, self.profit, self.weight, self.profitability)
