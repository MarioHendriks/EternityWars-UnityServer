using EternityWebsocket.Models;
using EternityWebsocket.Models.DTO;
using EternityWebsocket.Models.WsMessages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternityWebsocket.Logic
{
    class GameLogic
    {
        Random rnd = new Random();

        private void BroadCastGame(GameDTO gameDTO)
        {
            WsMessage wsMessage = new WsMessage();
            wsMessage.Action = "UpdateGame";
            wsMessage.Content = gameDTO;

            gameDTO.connectedPlayers[0].hero.mana = gameDTO.connectedPlayers[0].hero.maxMana;
            gameDTO.connectedPlayers[1].hero.mana = gameDTO.connectedPlayers[1].hero.maxMana;

            foreach (SocketClient client in Program.socketClients)
            {
                if(gameDTO.connectedPlayers[0] != null)
                {
                    if(client.userDTO.username == gameDTO.connectedPlayers[0].username)
                    {
                        Program.socketServer.SendClientMessage(client.socket, JsonConvert.SerializeObject(gameDTO));
                    }
                }
                if(gameDTO.connectedPlayers[1] != null)
                {
                    if (client.userDTO.username == gameDTO.connectedPlayers[1].username)
                    {
                        wsMessage.Content = SwapPlayers(gameDTO);
                        Program.socketServer.SendClientMessage(client.socket, JsonConvert.SerializeObject(gameDTO));
                    }
                }
            }
        }

        private GameDTO SwapPlayers(GameDTO gameDTO)
        {
            PlayerDTO player = gameDTO.connectedPlayers[0];
            gameDTO.connectedPlayers[0] = gameDTO.connectedPlayers[1];
            gameDTO.connectedPlayers[1] = player;
            return gameDTO;
        }

        private GameDTO IncreaseMaxMana(GameDTO game)
        {
            if(game.connectedPlayers[0].hero.maxMana != 10)
            {
                game.connectedPlayers[0].hero.maxMana = game.connectedPlayers[0].hero.maxMana + 1;
                return game;
            }
            return game;
        }

        private GameDTO IncreaseMaxDeathessence(GameDTO game)
        {
            if(game.connectedPlayers[0].hero.maxDeathessence != 10)
            {
                game.connectedPlayers[0].hero.maxDeathessence = game.connectedPlayers[0].hero.maxDeathessence + 1;
                return game;
            }
            return game;
        }

        public GameDTO RechargeMana(GameDTO game)
        {
            game.connectedPlayers[0].hero.mana = game.connectedPlayers[0].hero.maxMana;
            return game;
        }

        public void ConsumeResources(GameDTO game, int manaCost, int deathEssenceCost)
        {
            game.connectedPlayers[0].hero.mana = game.connectedPlayers[0].hero.mana - manaCost;
            game.connectedPlayers[0].hero.deathessence = game.connectedPlayers[0].hero.deathessence - deathEssenceCost;
        }



        public GameDTO ObtainDeathessence(GameDTO game)
        {
            int currentDeathessence = game.connectedPlayers[0].hero.deathessence;
            if(currentDeathessence < game.connectedPlayers[0].hero.maxDeathessence)
            {
                game.connectedPlayers[0].hero.deathessence = currentDeathessence + 1;
            }
            return game;
        }

        public GameDTO AttackCard()
        {
            GameDTO game = new GameDTO(); // get game from parameter
            int attacker = 1; // get attacker from parameter
            int target = 1; // get tartget from parameter

            if (!CheckForTurn(game))
            {
                return game;
            }

            CardSlotDTO attackersCardslot = game.connectedPlayers[0].boardrow.cardSlotList[attacker];
            CardSlotDTO targetCardslot = game.connectedPlayers[1].boardrow.cardSlotList[target];

            if (!attackersCardslot.card.isSleeping)
            {
                if (OpponentHasTaunt(game))
                {
                    if(TargetIsTaunt(game, target))
                    {
                        ProcessAttack(attackersCardslot, targetCardslot, game, target, attacker);
                    }else{
                        //send error function "You must target the card with taunt.")
                        return game;
                    }
                }else{
                    ProcessAttack(attackersCardslot, targetCardslot, game, target, attacker);
                }
                return game;
            }
            //send error function "This card is still asleep. Give it a turn to get ready."
            return game;
        }

        private void ProcessAttack(CardSlotDTO attacker, CardSlotDTO target, GameDTO game, int targetIndex, int attackerIndex)
        {
            target.card = CalculateRemainingHp(attacker.card, target.card);
            attacker.card = CalculateRemainingHp(target.card, attacker.card);

            if(target.card.health <= 0)
            {
                game.connectedPlayers[1].boardrow.cardSlotList[targetIndex].card = null;
                ObtainDeathessence(game);
            }else {
                game.connectedPlayers[1].boardrow.cardSlotList[targetIndex] = target;
            }

            if(attacker.card.health <= 0)
            {
                game.connectedPlayers[0].boardrow.cardSlotList[attackerIndex].card = null;  
            }else{
                game.connectedPlayers[0].boardrow.cardSlotList[attackerIndex] = attacker;
                attacker.card.isSleeping = true;
            }
        }

        public bool OpponentHasTaunt(GameDTO game)
        {
            for (int i = 0; i < game.connectedPlayers[1].boardrow.cardSlotList.Count(); i++)
            {
                if(!(game.connectedPlayers[1].boardrow.cardSlotList[i].card == null))
                {
                    if (game.connectedPlayers[1].boardrow.cardSlotList[i].card.taunt)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TargetIsTaunt(GameDTO game, int target)
        {
            if (game.connectedPlayers[1].boardrow.cardSlotList[target].card.taunt)
            {
                return true;
            }
            return false;
        }

        public GameDTO AttackHero()
        {
            GameDTO game = new GameDTO(); // get game from parameter
            int CardToAttackHeroWith = 1; // get attacker from parameter
            string token = "stoken"; // token to end send api call to end the game 

            if(game.playerTurn != game.connectedPlayers[0].userId)
            {
                //send error function "This isn't your turn"
                return game;
            }

            CardDTO attackerCard = game.connectedPlayers[0].boardrow.cardSlotList[CardToAttackHeroWith].card;
            if (!attackerCard.isSleeping)
            {
                if (!OpponentHasTaunt(game))
                {
                    int currentHp = game.connectedPlayers[1].hero.hp;
                    game.connectedPlayers[1].hero.hp = currentHp - attackerCard.attack;
                    attackerCard.isSleeping = true;
                    if(game.connectedPlayers[1].hero.hp <= 0)
                    {
                        //function to end game 
                        //TODO check implementatie
                        EndGame(game);
                    }
                    return game;
                }
                else
                {
                    //send error function "You must target the card with taunt."
                }
                return game;
            }
            //send error function  "This card is still asleep. Give it a turn to get ready."
            return game;
        }

        public void RemoveCardFromHand(GameDTO game, int CardToPlay)
        {
            game.connectedPlayers[0].cardsInHand.RemoveAt(CardToPlay);
        }

        public GameDTO PlayCard()
        {
            GameDTO game = new GameDTO(); // get game from parameter
            int cardToPlay = 1; // get cardtoplay from parameter
            int target = 1 ; // get target on the field from card to play

            if (!CheckForTurn(game))
            {
                return game;
            }

            int currentMana = game.connectedPlayers[0].hero.mana;
            int currentDeathEssence = game.connectedPlayers[0].hero.deathessence;
            CardDTO playableCard = game.connectedPlayers[0].cardsInHand[cardToPlay];

            if(currentMana < playableCard.blue_mana)
            {
                //send error function "You dont have enough resources to do that!"
                return game;
            }
            if (currentDeathEssence < playableCard.death_essence)
            {
                //send error function "You dont have enough resources to do that!"
                return game;
            }

            ConsumeResources(game, playableCard.blue_mana, playableCard.death_essence);
            RemoveCardFromHand(game, cardToPlay);
            game.connectedPlayers[0].boardrow.cardSlotList[target].card = playableCard;

            return game;
        }

        private void EndGame(GameDTO game)// add token to parameter if this server needs to add gold to player
        {
            SendGame(game, "EndGame");

            //make functions to add gold into db
        }

        private CardDTO CalculateRemainingHp(CardDTO attacker, CardDTO target)
        {
            target.health = target.health = attacker.attack;
            return target;
        }

        private GameDTO DrawCard(GameDTO game)
        {
            DeckDTO pickableDeck = game.connectedPlayers[1].deck;
            int cardId = rnd.Next(0, pickableDeck.cards.cards.Count());
            CardDTO card = pickableDeck.cards.cards[cardId];

            if (CanDraw(game))
            {
                pickableDeck.cards.cards.RemoveAt(cardId);
                game.connectedPlayers[1].cardsInHand.Add(card);
                game.connectedPlayers[1].cardsInDeck = pickableDeck.cards.cards;
                return game;
            }
            //send error function "Your hand is full!"
            return game;
        }

        private bool CanDraw(GameDTO game)
        {
            if(game.connectedPlayers[1].cardsInHand.Count() < 9)
            {
                return true;
            }
            return false;
        }

        public void SendGame(GameDTO game, string action)
        {
            WsMessage gameState = new WsMessage();
            gameState.Content = game;
            gameState.Action = action;

            string data = JsonConvert.SerializeObject(gameState);

            foreach (SocketClient client in Program.socketClients)
            {
                if(game.connectedPlayers[0] != null)
                {
                    if(client.userDTO.username == game.connectedPlayers[0].username)
                    {
                        Program.socketServer.SendClientMessage(client.socket, data);
                    }
                }
                if(game.connectedPlayers[1] != null)
                {
                    if(client.userDTO.username == game.connectedPlayers[1].username)
                    {
                        gameState.Content = SwapPlayers(game);
                        Program.socketServer.SendClientMessage(client.socket, data);
                    }
                }
            }
        }

        public void AwakeCard(GameDTO game)
        {
            foreach (CardSlotDTO cardSlot in game.connectedPlayers[1].boardrow.cardSlotList)
            {
                if(cardSlot.card != null)
                {
                    cardSlot.card.isSleeping = false;
                }
            }
        }

        private bool CheckForTurn(GameDTO game)
        {
            if(game.playerTurn != game.connectedPlayers[0].userId)
            {
                //send error function "This isn't your turn"
                return false;
            }
            return true;
        }
    }
}
