from google.appengine.ext import ndb
from LongProperty import LongIntegerProperty

class SetupData(ndb.Model):
    setup_complete = ndb.BooleanProperty()


class Account(ndb.Model):
    username = ndb.StringProperty()
    email = ndb.StringProperty()
    uniquekey = ndb.StringProperty()
    solarsystem = ndb.StringProperty()
    currentplanet = ndb.StringProperty()
    joined = ndb.DateTimeProperty(auto_now_add=True)
    acctivationhash = ndb.StringProperty()
    active = ndb.BooleanProperty()
    currentAccessToken = ndb.KeyProperty()

class AccessToken(ndb.Model):
    account_key = ndb.KeyProperty()
    access_token_key = ndb.StringProperty()
    login_time = ndb.DateTimeProperty()

class PlanetAccountInfo(ndb.Model):
    name = ndb.StringProperty()
    completed = ndb.BooleanProperty()
    account_info = ndb.KeyProperty()
    traveled_distance = ndb.FloatProperty(default=0)
    date_of_completion = ndb.DateTimeProperty(default=None)
    date_of_starting = ndb.DateTimeProperty(default=None)
    game_time = ndb.FloatProperty()
    started = ndb.BooleanProperty(default=False)

class Planet(ndb.Model):
    distance = ndb.FloatProperty(default=0)
    name = ndb.StringProperty()
    index = ndb.IntegerProperty()
    planet_info_list = ndb.KeyProperty(repeated=True)

class Score():
    def __init__(self,username,time,score):
        self.username = username
        self.time = time
        self.score = score

class SolarSystem(ndb.Model):
    name = ndb.StringProperty()
    planets = ndb.KeyProperty(repeated=True)
    completed = ndb.KeyProperty(repeated=True)

class SolarSystemDic():
    def __init__(self,name):
        self.name = name
        self.planets = []

class TempUser():
    def __init__(self, uniquekey, email,username=''):
        self.uniquekey = uniquekey
        self.email = email
        self.username = username
