#!/usr/bin/env python
#
# Copyright 2007 Google Inc.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
#

import cgi
import urllib
import urllib2
import json
import logging
from google.appengine.ext import ndb
from google.appengine.api import users
from google.appengine.api import urlfetch
from google.appengine.api import mail
from containers import *
import webapp2
import hashlib
import string
import random
import datetime

# check if user exists
def CheckIfUserIsValid(user):
    temp_user = Account.query().filter(Account.uniquekey == user.uniquekey,Account.email==user.email,Account.active==True).get()
    if temp_user is None:
        return False
    return True

def GetScoresForCurrentPlanet(planet):
    scores = []
    planet_account_info_keys = planet.planet_info_list

    q = PlanetAccountInfo.query().filter(PlanetAccountInfo.name == planet.name).order(PlanetAccountInfo.game_time)

    score_list = q.fetch(10)

    logging.info(score_list)

    for score in score_list:
        logging.info(score)
        if score.date_of_completion is not None:

            m , s = divmod(score.game_time,60)

            h , m = divmod(m,60)

            logging.info('%d + %d + %d',s,m,h)

            game_time_string = '{0}-{1}-{2}'.format(h,m,s)

            logging.info(game_time_string)

            logging.info('adding score')
            scores.append({'username':score.account_info.get().username,'time':game_time_string,'distancetraveled':score.traveled_distance})
        else:
            logging.info('no score to add')
            continue
    return scores

def CheckIfUserIsLoggedIn(user):
    if user.currentAccessToken != None:
        return True
    return False

def GetUser(user):
    return Account.query().filter(Account.uniquekey == user.uniquekey, Account.email == user.email).get()

def CheckIfUserExists(user):
    #check if any account have the same email
    email_user = Account.query().filter(Account.email==user.email).get()
    if email_user is None:
        #check if any account have the same name
        name_user = Account.query().filter(Account.username==user.username).get()
        if name_user is None:
            return 'CREATE_ACCOUNT_CONFIRMED'
        else:
            return 'USERNAME_ALREADY_EXISTS_ERROR'
    else:
        return 'EMAIL_ALREADY_EXISTS_ERROR'

def CheckHashedLinkValidation(hashedlink,map):
    return Account.query().filter(Account.acctivationhash==hashedlink,Account.uniquekey==map,Account.active==False).get()

def CheckIfAccountTokenIsValid(accessToken):
    return AccessToken.query().filter(AccessToken.access_token_key==accessToken).get()

def SolarSystemExists(name):
    solar_system = SolarSystem.query().filter(SolarSystem.name == name).get()
    if solar_system is None:
        return False
    return True

def id_generator(size=8, chars=string.ascii_uppercase + string.digits):
    return ''.join(random.SystemRandom().choice(string.ascii_uppercase + string.digits) for _ in range(size))

def CreateUser(user):
    logging.info(user)
    temp = Account(username=user.username,uniquekey=user.uniquekey, email=user.email, solarsystem='SolarSystem', currentplanet='Moon',acctivationhash=hashlib.sha224(id_generator()).hexdigest(),active=False,currentAccessToken=None)
    return temp.put()

def ParseSolarSystem(solarsystem):
    logging.info("in parsing")
    solarsystem_dict = SolarSystemDic(solarsystem.name)
    for key in solarsystem.planets:
        planet = key.get()
        solarsystem_dict.planets.append({'distance': planet.distance, 'name': planet.name, 'index': planet.index})
        logging.info(solarsystem_dict.planets)
    return solarsystem_dict

def GetNextSolarSystem(user):
    if user.solarsystem == None:
        # create a random solarsystem
        return
    else:
        # create a random solarsystem
        return

def FetchSolarSystem(user):
    logging.info('fetching solar system : '+user.solarsystem)
    solar_system = SolarSystem.query(SolarSystem.name == user.solarsystem)
    logging.info(solar_system)
    return solar_system.get()

def IsSolarSystemCompleted(user):
    logging.info(user)
    solar_system = FetchSolarSystem(user)
    logging.info('Checking if Player completed solarsystem')
    for planet_key in solar_system.planets:
        planet = planet_key.get()
        if len(planet.planet_info_list) > 1:
            logging.info(len(planet.planet_info_list))
            # gives true if list is not empty
            logging.info('planet list is not empty for planet: '+planet.name+' user: '+user.uniquekey)
            logging.info('checking if player completed the planet')
            for planet_info_key in planet.planet_info_list:
                if planet_info_key.get().account_info is not None:
                    if planet_info_key.get().account_info.get() == user:
                        if planet_info_key.get().completed == True:
                            logging.info('Completed planet:'+planet.name)
                            break
                        else:
                            logging.info('Is still not complete with planet')
                            logging.info('Solarsystem is not yet complete')
                            return False
                else:
                    continue
            else:
                continue
        else:
            logging.info('Solarsystem is not yet complete')
            return False
    else:
        return True

def GetGameData(user):
    if SolarSystemExists(user.solarsystem) is True: #checks the solar system exists
        if IsSolarSystemCompleted(user): # if true then go to the next solarsystem
            return GetNextSolarSystem(user)
        else: #if false then make sure he is on the right one, by planet index.
            logging.info('Getting Game Data for :'+ user.username)
            AddUserToSolarSystem(user)
            return ParseSolarSystem(FetchSolarSystem(user))
    else: # solar system does nt exists, go to the next one
        return GetNextSolarSystem(user)

def AddUserToSolarSystem(user):
    solar_system = FetchSolarSystem(user)
    logging.info('Retrieved solar system : '+solar_system.name)
    logging.info('Checking all keys')
    for planet_key in solar_system.planets:
        planet = planet_key.get()
        if planet.name == user.currentplanet:  # check if current planet has same name as the iterator planet name
            logging.info('found correct key')
            logging.info(planet.planet_info_list)
            if planet.planet_info_list:  # non empty list
                logging.info('list is not empty')
                logging.info('checking planet info keys')
                for planet_info_key in planet.planet_info_list:  # for iterator through list of planet info keys
                    if planet_info_key.get().account_info is not None:
                        if planet_info_key.get().account_info.get() == user:  # same as user then return true
                            logging.info('user already exists in list')
                            return False  # User exists
                        else:
                            continue
                    else:
                        continue
                else:  # user not found in list, add to list
                    logging.info('Did not found user in list , adding to list')
                    temp_info = PlanetAccountInfo(name=user.currentplanet,completed=False, account_info=user.key, traveled_distance=0)
                    temp_info_key = temp_info.put()
                    planet.planet_info_list.append(temp_info_key)
                    planet.put()
                    logging.info(planet.planet_info_list)
                    return True  # User added
            else:  # if empty list then set user in the list
                logging.info('list is empty')
                logging.info('adding data')
                temp_info = PlanetAccountInfo(name=user.currentplanet,completed=False, account_info=user.key, traveled_distance=0)
                temp_info_key = temp_info.put()
                planet.planet_info_list.append(temp_info_key)
                planet.put()
                return True
    else:
        logging.debug("Grave Error when trying yo add User to solarsystem")
        raise NameError("GRAVE ERROR FIX")

def AddUserToSolarSystemInit(user,solar_system_key):
    solar_system = solar_system_key.get()
    logging.info('Retrieved solar system : '+solar_system.name)
    logging.info('Checking all keys')
    for planet_key in solar_system.planets:
        planet = planet_key.get()
        if planet.name == user.currentplanet:  # check if current planet has same name as the iterator planet name
            logging.info('found correct key')
            logging.info(planet.planet_info_list)
            if planet.planet_info_list:  # non empty list
                logging.info('list is not empty')
                logging.info('checking planet info keys')
                for planet_info_key in planet.planet_info_list:  # for iterator through list of planet info keys
                    if planet_info_key.get().account_info is not None:
                        if planet_info_key.get().account_info.get() == user:  # same as user then return true
                            logging.info('user already exists in list')
                            return False  # User exists
                        else:
                            continue
                    else:
                        continue
                else:  # user not found in list, add to list
                    logging.info('Did not found user in list , adding to list')
                    temp_info = PlanetAccountInfo(name=user.currentplanet,completed=False, account_info=user.key, traveled_distance=0)
                    temp_info_key = temp_info.put()
                    planet.planet_info_list.append(temp_info_key)
                    planet.put()
                    logging.info(planet.planet_info_list)
                    return True  # User added
            else:  # if empty list then set user in the list
                logging.info('list is empty')
                logging.info('adding data')
                temp_info = PlanetAccountInfo(name=user.currentplanet,completed=False, account_info=user.key, traveled_distance=0)
                temp_info_key = temp_info.put()
                planet.planet_info_list.append(temp_info_key)
                planet.put()
                return True
    else:
        logging.debug("Grave Error when trying yo add User to solarsystem")
        raise NameError("GRAVE ERROR FIX")

def CreateDummy():
    return PlanetAccountInfo(completed=False).put()

def CreateDefaultSolarSystem(dummy):

    mercury = Planet(distance=91652559700.0, name='Mercury', index=1, planet_info_list=[dummy])
    venus = Planet(distance=50290000000.0, name='Venus', index=2, planet_info_list=[dummy])
    moon = Planet(distance=384403000.0, name='Moon', index=0, planet_info_list=[dummy])
    mars = Planet(distance=119740000000.0, name='Mars', index=3, planet_info_list=[dummy])
    jupiter = Planet(distance=720420000000.0, name='Jupiter', index=4, planet_info_list=[dummy])
    saturn = Planet(distance=646270000000.0, name='Saturn', index=5, planet_info_list=[dummy])
    uranus = Planet(distance=1448950000000.0, name='Uranus', index=6, planet_info_list=[dummy])
    neptune = Planet(distance=1627450000000.0, name='Neptune', index=7, planet_info_list=[dummy])
    pluto = Planet(distance=4265932000000.0, name='Pluto', index=8, planet_info_list=[dummy])

    mercury_key = mercury.put()
    venus_key = venus.put()
    moon_key = moon.put()
    mars_key = mars.put()
    jupiter_key = jupiter.put()
    saturn_key = saturn.put()
    uranus_key = uranus.put()
    neptune_key = neptune.put()
    pluto_key = pluto.put()

    dummy_account = Account(uniquekey='dummy')
    dummy_account_key = dummy_account.put()
    solarSystem = SolarSystem(name='SolarSystem',
                              planets=[mercury_key, venus_key, moon_key, mars_key, jupiter_key, saturn_key, uranus_key,
                                       neptune_key, pluto_key],
                              completed=[dummy_account_key])
    return solarSystem.put()

def FirstTimeSetup():
    if SetupData.query().filter(SetupData.setup_complete == True).get() is None:
        # do first time setup like setting up the dummy and solar system
        dummy = CreateDummy()
        solar_system_key = CreateDefaultSolarSystem(dummy)
        SetupData(setup_complete=True).put()
        logging.info('Setup Complete')
        return solar_system_key
    else:
        return

def StartCurrentPlanet(user):

    logging.info(user)

    planet_account_info = PlanetAccountInfo.query().filter(PlanetAccountInfo.account_info == user.key,PlanetAccountInfo.completed == False,PlanetAccountInfo.started == False).get()

    logging.info(planet_account_info)

    if planet_account_info is None:
        logging.info('setup already complete or found none')
        return

    planet_account_info.date_of_starting = datetime.datetime.today()
    planet_account_info.started = True
    planet_account_info.put()

def UpdatePlanetInfo(new_info,user):
    planet_account_info = PlanetAccountInfo.query().filter(PlanetAccountInfo.account_info == user.key, PlanetAccountInfo.completed == False , PlanetAccountInfo.date_of_starting != None).get()

    logging.info(planet_account_info)

    logging.info(new_info)

    if planet_account_info is None:
        return

    planet_account_info.traveled_distance = float(unicode(new_info['traveled_distance']))


    logging.info(new_info['completed'])

    if new_info['completed'] == 'true':
        planet_account_info.completed = True
        planet_account_info.date_of_completion = datetime.datetime.today()
        planet_account_info.game_time = (planet_account_info.date_of_completion - planet_account_info.date_of_starting).total_seconds()
    else:
        planet_account_info.completed = False

    planet_account_info.put()

    logging.info(planet_account_info)
    logging.info(planet_account_info.completed)

    if planet_account_info.completed is True:
        # change to the next planet, calculate the time it took for him to play this game

        if IsSolarSystemCompleted(user):
            logging.info('completed solar system')
            # solarsystem is complete switch to next one
            return
        else:
            logging.info('solarsystem not complete , go to next planet')
            # go to next planet
            NextPlanet(user)
            return

def NextPlanet(user):

    solarsystem = SolarSystem.query().filter(SolarSystem.name == user.solarsystem).get()

    old_index = 0
    new_index = 0

    for planet_key in solarsystem.planets:
        if planet_key.get().name == user.currentplanet:
            old_index = planet_key.get().index
            new_index = old_index+1
            break

    for planet_key in solarsystem.planets:
        if planet_key.get().index == new_index:
            user.currentplanet = planet_key.get().name
            user.put()
            temp_info = PlanetAccountInfo(name=user.currentplanet,completed=False,account_info=user.key,traveled_distance=0)
            temp_info_key = temp_info.put()
            planet_key.get().planet_info_list.append(temp_info_key)
            planet_key.get().put()
            break
    return

class Activate(webapp2.RequestHandler):
    def get(self):
        logging.info(self.request.query_string)
        hashedlink = self.request.get('link')
        logging.info(hashedlink+' : HASHED LINK')
        map = self.request.get('map')
        logging.info(map+' : MAP')
        logging.info('A user tries to validate')
        user_ = CheckHashedLinkValidation(hashedlink,map)
        logging.info(user_)
        if user_ is  None:
            #do nothing
            logging.info('false validation')
            return
        else:
            #validate account
            user_.active = True
            user_.put()
            self.response.out.write('Validated, Welcome to TakeMeToTheMoon')
            return

#checks access token, if correct then continue
class GetSolarSystem(webapp2.RequestHandler):
    def post(self):
        accessToken = self.request.get('AccessToken')
        logging.info(accessToken)
        accessToken_user = CheckIfAccountTokenIsValid(accessToken)
        logging.info(accessToken_user)
        if accessToken_user is None:
            self.response.out.write('Wrong Login Sequence')
            return
        else:
            user = GetUser(accessToken_user.account_key.get())
            logging.info(user)
            solar_system = GetGameData(user)
            self.response.headers['Content-Type']='application/json'
            obj={'solarsystem':{'name':solar_system.name,'planets':solar_system.planets},'currentplanet':user.currentplanet}
            self.response.out.write(json.dumps(obj))
            return
# This function will first check if the current user with pass is in the database, if user exists then receive a access token
class Login(webapp2.RequestHandler):
    def post(self):
        #check if data is valid
        temp_user = TempUser(self.request.get('UserKey'), self.request.get('Email'))
        # check if user exists and if validated
        if CheckIfUserIsValid(temp_user) is False:
            logging.info('Login failed by user: '+temp_user.email)
            self.response.out.write('Credentials false , Login failed')
            return
        else:
            logging.info(temp_user)
            user = GetUser(temp_user)
            logging.info(user)
            if CheckIfUserIsLoggedIn(user) is True:
                #Send error message
                logging.info('User is already logged in: '+user.email)
                self.response.out.write('User Already Logged In')
            else:
                #CREATE A TOKEN FOR A 1 HOUR DURATION
                user = GetUser(temp_user)
                time = datetime.datetime.now()
                time_of_usage = time+datetime.timedelta(hours=1)
                access_token = AccessToken(account_key=user.key,access_token_key=hashlib.sha224(id_generator()).hexdigest(),login_time=time_of_usage)
                access_token.put()
                user.currentAccessToken = access_token.key
                user.put()
                logging.info('Login successful by user: '+temp_user.email)
                self.response.out.write(access_token.access_token_key)
                return
class Register(webapp2.RequestHandler):
    def post(self):
        logging.info(self.request.host_url)
        solar_system_key = FirstTimeSetup()
        #check if data is valid
        if solar_system_key is None:
            temp_user = TempUser(self.request.get('UserKey'),self.request.get('Email'),self.request.get('Username'))
            confirmation = CheckIfUserExists(temp_user)
            if confirmation == 'EMAIL_ALREADY_EXISTS_ERROR':
                logging.info('Account Creation Failed : Reason : '+confirmation)
                self.response.out.write('Email already exists')
                return
            if confirmation == 'USERNAME_ALREADY_EXISTS_ERROR':
                logging.info('Account Creation Failed : Reason : '+confirmation)
                self.response.out.write('Username already exists')
                return
            if confirmation == 'CREATE_ACCOUNT_CONFIRMED':
                #create user
                user_key = CreateUser(temp_user)
                user_ = user_key.get()
                AddUserToSolarSystem(user_)
                logging.info('Account created, Account: name '+ user_.username+' Email: '+user_.email)
                #send confirmation mail
                confirmation_url = self.request.host_url+'/activate?link='+user_.acctivationhash+'&map='+user_.uniquekey
                sender_address = "Example.com Support <support@example.com>"
                subject = "Confirm your registration"
                to = '{0} <{1}>'.format(user_.username,user_.email)
                body = """
                Thank you for creating an account! Please confirm your email address by
                clicking on the link below:
                %s""" % confirmation_url
                mail.send_mail(sender_address,to,subject,body)
                logging.info('Sending Confirmation Email '+confirmation_url)
                self.response.out.write('Account Created')
                return
        else:
            temp_user = TempUser(self.request.get('UserKey'),self.request.get('Email'),self.request.get('Username'))
            confirmation = CheckIfUserExists(temp_user)
            if confirmation == 'EMAIL_ALREADY_EXISTS_ERROR':
                logging.info('Account Creation Failed : Reason : '+confirmation)
                self.response.out.write('Email already exists')
                return
            if confirmation == 'USERNAME_ALREADY_EXISTS_ERROR':
                logging.info('Account Creation Failed : Reason : '+confirmation)
                self.response.out.write('Username already exists')
                return
            if confirmation == 'CREATE_ACCOUNT_CONFIRMED':
                #create user
                user_key = CreateUser(temp_user)
                user_ = user_key.get()
                AddUserToSolarSystemInit(user_,solar_system_key)
                logging.info('Account created, Account: name '+ user_.username+' Email: '+user_.email)
                 #send confirmation mail
                confirmation_url = self.request.host_url+'/activate?link='+user_.acctivationhash+'&map='+user_.uniquekey
                sender_address = "Example.com Support <support@example.com>"
                subject = "Confirm your registration"
                to = '{0} <{1}>'.format(user_.username,user_.email)
                body = """
                Thank you for creating an account! Please confirm your email address by
                clicking on the link below:
                %s""" % confirmation_url
                mail.send_mail(sender_address,to,subject,body)
                logging.info('Sending Confirmation Email '+confirmation_url)
                self.response.out.write('Account Created')
                return
class GetHighScore(webapp2.RequestHandler):
    def post(self):
        #accesstoken check
        accessToken = self.request.get('AccessToken')

        if accessToken is '':
            return

        logging.info(accessToken)
        accessToken_user = CheckIfAccountTokenIsValid(accessToken)
        logging.info(accessToken_user)

        planet = Planet.query().filter(Planet.name == accessToken_user.account_key.get().currentplanet)

        if planet is None:
            logging.info('Error retrieving high score')
            self.response.out.write("Error retrieving high score")
            return

        scores = GetScoresForCurrentPlanet(planet.get())

        if len(scores) is 0:
            logging.info('Nothing to give')
            self.response.out.write('Nothing to give')
            return

        logging.info('Sending HighScore')
        self.response.out.write(json.dumps(scores))
        return
class HourlyServerCheck(webapp2.RequestHandler):
    def get(self):
        logging.info('HEJ')
        return
class SendData(webapp2.RequestHandler):
    def post(self):
        #accesstoken check
        accessToken = self.request.get('AccessToken')

        if accessToken is '':
            return

        logging.info(accessToken)
        accessToken_user = CheckIfAccountTokenIsValid(accessToken)
        logging.info(accessToken_user)

        if accessToken_user is None:
            logging.info('AccessToken wrong')
            self.response.out.write('AccessToken Wrong')
            return

        logging.info('AccessToken correct')

        if (self.request.get('StartingCurrentPlanet') is not ''):
            logging.info('Setting Up Start Setup')
            StartCurrentPlanet(accessToken_user.account_key.get())
            self.response.out.write('Startup Complete')
            return

        if (self.request.get('UpdatePlanetInfo') is not ''):
            object = json.loads(self.request.get('UpdatePlanetInfo'))
            UpdatePlanetInfo(object,accessToken_user.account_key.get())
            self.response.out.write('Saved Data to Database')
            return
        if (self.request.get('CompletedCurrentPlanet') is not ''):
            object = json.loads(self.request.get('CompletedCurrentPlanet'))
            UpdatePlanetInfo(object,accessToken_user.account_key.get())
            self.response.out.write('Saved CompletedData to Database, and going to the next planet')
            return

app = webapp2.WSGIApplication([
    ('/login', Login),
    ('/getsolarsystem', GetSolarSystem),
    ('/register',Register),
    ('/gethighscore',GetHighScore),
    ('/activate',Activate),
    ('/HS',HourlyServerCheck),
    ('/senddata',SendData)
], debug=True)
