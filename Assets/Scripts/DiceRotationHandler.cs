using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRotationHandler : MonoBehaviour
{
    public Transform Dice1;
    public Transform Dice2;
    public static Animator DicesAC;

    public List<Vector3> DiceResultRotations = new();
    int dice1Value = 0;
    int dice2Value = 0;
    int Sum = 0;
    Vector3 Dice1StartPos = new Vector3();
    Vector3 Dice2StartPos = new Vector3();

    private void Awake()
    {
        DicesAC = GetComponent<Animator>();
        Dice1StartPos = Dice1.parent.position;
        Dice2StartPos = Dice2.parent.position;
    }

    [ContextMenu("Roll Dices")]
    public void RollDices()
    {
        DicesAC.enabled = true;
        DicesAC.Play("Roll");
    }

    public void ShowDiceResult()
    {
        Quaternion dice1Rot = Dice1.localRotation;
        Quaternion dice2Rot = Dice2.localRotation;
        DicesAC.enabled = false;
        Dice1.localRotation = dice1Rot;
        Dice2.localRotation = dice2Rot;
        int dice1Face = 0;
        int dice2Face = 0;
        /*if (!APIController.instance.userDetails.isBlockApiConnection)
        {
            if (DiceRolla.DiceRoll.winProbability > 0)
            {
                Debug.Log("Normal Randomness Condition Executed");
                Debug.Log("RNG Calculation:\n==============\nTrue\n==============\n");
                dice1Face = Random.Range(0, DiceResultRotations.Count);
                dice2Face = Random.Range(0, DiceResultRotations.Count);
            }
            else if (DiceRolla.DiceRoll.winProbability == 0)
            {
                Debug.Log("RNG Calculation:\n==============\nFalse\n==============\n");
                int count = 0;
                bool TwoOnSix = (BetsManager.Bets.BettingPools[0] > 0 || BetsManager.Bets.BettingPools[3] > 0 || BetsManager.Bets.BettingPools[4] > 0 || BetsManager.Bets.BettingPools[5] > 0 || BetsManager.Bets.BettingPools[6] > 0 || BetsManager.Bets.BettingPools[7] > 0) && count < 13;
                bool EightOnTwelve = (BetsManager.Bets.BettingPools[2] > 0 || BetsManager.Bets.BettingPools[8] > 0 || BetsManager.Bets.BettingPools[9] > 0 || BetsManager.Bets.BettingPools[10] > 0 || BetsManager.Bets.BettingPools[11] > 0 || BetsManager.Bets.BettingPools[12] > 0) && count < 13;
                bool OnSeven = BetsManager.Bets.BettingPools[1] > 0;
                foreach (int value in BetsManager.Bets.BettingPools) if (value > 0) count++;
                if (count == 12 && TwoOnSix && EightOnTwelve && OnSeven)
                {
                    if (count == 12) Debug.Log("All Bets Spamming Condition executed");
                    dice1Face = Random.Range(0, DiceResultRotations.Count - 1);
                    switch (dice1Face)
                    {
                        case 0:
                            dice2Face = 5;//dice 1 = 1
                            break;
                        case 1:
                            dice2Face = 4;// dice 1 = 2
                            break;
                        case 2:
                            dice2Face = 3;// dice 1 = 3
                            break;
                        case 3:
                            dice2Face = 2;// dice 1 = 4
                            break;
                        case 4:
                            dice2Face = 1;// dice 1 = 5
                            break;
                    }
                }
                else if (TwoOnSix && EightOnTwelve && OnSeven && count < 12)
                {
                    Debug.Log("Spamming bets across all three ranges except for the ranges themselves Condition executed");
                    for (int value = 3; value <= 12; value++)
                    {
                        if (BetsManager.Bets.BettingPools[value] == 0)
                        {
                            switch (value)
                            {
                                case 3: //value 2
                                    dice1Face = 0;
                                    dice2Face = 0;
                                    Debug.Log("Altered Result: 2");
                                    break;
                                case 4: //value 3
                                    dice1Face = 1;
                                    dice2Face = 0;
                                    Debug.Log("Altered Result: 3");
                                    break;
                                case 5: //value 4
                                    dice1Face = Random.Range(0, 3);
                                    dice2Face = (4 - (dice1Face + 1)) - 1;
                                    Debug.Log("Altered Result: 4");
                                    break;
                                case 6: //value 5
                                    dice1Face = Random.Range(0, 4);
                                    dice2Face = (5 - (dice1Face + 1)) - 1;
                                    Debug.Log("Altered Result: 5");
                                    break;
                                case 7: //value 6
                                    dice1Face = Random.Range(0, 5);
                                    dice2Face = (6 - (dice1Face + 1)) - 1;
                                    Debug.Log("Altered Result: 6");
                                    break;
                                case 8: //value 8
                                    dice1Face = Random.Range(0, 7);
                                    dice2Face = (8 - (dice1Face + 1)) - 1;
                                    Debug.Log("Altered Result: 3");
                                    break;
                                case 9: //value 9
                                    dice1Face = Random.Range(0, 7);
                                    dice2Face = (9 - (dice1Face + 1)) - 1;
                                    break;
                                case 10: //value 10
                                    dice1Face = Random.Range(0, 7);
                                    dice2Face = (10 - (dice1Face + 1)) - 1;
                                    break;
                                case 11: //value 11
                                    dice1Face = 5;
                                    dice2Face = 4;
                                    break;
                                case 12: //value 12
                                    dice1Face = 5;
                                    dice2Face = 5;
                                    break;
                            }
                            break;
                        }
                    }
                    *//*                    switch (dice1Face)
                                        {
                                            case 0:
                                                dice2Face = 5;//dice 1 = 1
                                                break;
                                            case 1:
                                                dice2Face = 4;// dice 1 = 2
                                                break;
                                            case 2:
                                                dice2Face = 3;// dice 1 = 3
                                                break;
                                            case 3:
                                                dice2Face = 2;// dice 1 = 4
                                                break;
                                            case 4:
                                                dice2Face = 1;// dice 1 = 5
                                                break;
                                        }*//*
                }
                else if (TwoOnSix && !EightOnTwelve)
                {
                    Debug.Log("2-6 Condition executed");
                    if (!OnSeven) dice1Face = Random.Range(0, DiceResultRotations.Count);
                    else dice1Face = Random.Range(1, DiceResultRotations.Count);
                    switch (dice1Face)
                    {
                        case 0:
                            dice2Face = 5;//dice 1 = 1
                            break;
                        case 1:
                            dice2Face = Random.Range(4, DiceResultRotations.Count);// dice 1 = 2
                            break;
                        case 2:
                            dice2Face = Random.Range(3, DiceResultRotations.Count);// dice 1 = 3
                            break;
                        case 3:
                            dice2Face = Random.Range(2, DiceResultRotations.Count);// dice 1 = 4
                            break;
                        case 4:
                            dice2Face = Random.Range(1, DiceResultRotations.Count);// dice 1 = 5
                            break;
                        case 5:
                            dice2Face = Random.Range(0, DiceResultRotations.Count);// dice 1 = 6
                            break;
                    }
                }
                else if (EightOnTwelve && !TwoOnSix)
                {
                    Debug.Log("8-12 Condition executed");
                    if (!OnSeven) dice1Face = Random.Range(0, DiceResultRotations.Count - 1);
                    else dice1Face = Random.Range(1, DiceResultRotations.Count - 1);
                    switch (dice1Face)
                    {
                        case 0:
                            dice2Face = Random.Range(0, 6);//dice 1 = 1
                            break;
                        case 1:
                            dice2Face = Random.Range(0, 5);// dice 1 = 2
                            break;
                        case 2:
                            dice2Face = Random.Range(0, 4);// dice 1 = 3
                            break;
                        case 3:
                            dice2Face = Random.Range(0, 3);// dice 1 = 4
                            break;
                        case 4:
                            dice2Face = Random.Range(0, 2);// dice 1 = 5
                            break;
                    }
                }
            }
        }*/
        Debug.Log("Demo Dice Result Outcome");
        dice1Face = Random.Range(0, DiceResultRotations.Count);
        dice2Face = Random.Range(0, DiceResultRotations.Count);
        Vector3 dice1Result = DiceResultRotations[dice1Face];
        dice1Result.y = Random.Range(0, 360);
        Dice1.parent.position = Dice1StartPos;
        Dice1.DOLocalRotate(dice1Result, 0.1f);
        Vector3 dice2Result = DiceResultRotations[dice2Face];
        dice2Result.y = Random.Range(0, 360);
        //Debug.Log($"Dice2 result rotation is {dice2Result}");
        Dice2.parent.position = Dice2StartPos;
        Dice2.DOLocalRotate(dice2Result, 0.1f);
        dice1Value = GetDiceValue(dice1Result);
        dice2Value = GetDiceValue(dice2Result);
        Sum = dice1Value + dice2Value;
        //Debug.Log($"Dice 2 value: {dice2Value}");
        DiceRolla.DiceRoll.UpdateHistory(dice1Value, dice2Value, Sum);
    }

    public int GetDiceValue(Vector3 diceAngle)
    {
        if (diceAngle.x == 0 && diceAngle.z == 0) return 1;
        else if (diceAngle.z == -90 && diceAngle.x == 0) return 2;
        else if (diceAngle.z == 0 && diceAngle.x == 90) return 3;
        else if (diceAngle.z == 0 && diceAngle.x == -90) return 4;
        else if (diceAngle.z == 90 && diceAngle.x == 0) return 5;
        else if (diceAngle.x == 0 && diceAngle.z == 180) return 6;
        else return 0;
    }
}