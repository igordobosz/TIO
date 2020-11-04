class Population():
    def __init__(self,index, thievefs):
        self.index = index
        self.thievefs = thievefs
        self.avg = 0
        self.best_thief = thievefs[0]
        self.worst_thief = thievefs[0]

    def __str__(self):
        return "Population: [index = {}, best_thief = {}, avg = {}, worst_thief = {}]".format(self.index, self.best_thief.fitness, self.avg,self.worst_thief.fitness)

    def getCsvString(self):
        return "{}, {}, {}, {}\n".format(self.index, int(self.best_thief.fitness), int(self.avg), int(self.worst_thief.fitness))

    def calcParameters(self):
        best_thief = self.thievefs[0]
        worst_thief = self.thievefs[0]
        sum = 0
        for thief in self.thievefs:
            if thief.fitness > best_thief.fitness:
                best_thief = thief
            if thief.fitness < worst_thief.fitness:
                worst_thief = thief
            sum += thief.fitness
        self.avg = sum/self.thievefs.__len__()
        self.best_thief = best_thief
        self.worst_thief = worst_thief
