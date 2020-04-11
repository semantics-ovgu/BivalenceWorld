using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameRuleSet
{
    private const string AllQuantum = "\u2200";
    private const string ExistQuantum = "\u2203";
    private const string Implication = "\u2192";
    private const string AND = "\u2227";
    private const string OR = "\u2228";


    public static List<string> GetRule(ERule rule)
    {
        switch (rule)
        {
            case ERule.SelectionPossible:
                return new List<string>
                {
                    $"{AllQuantum}x (Selected(x) {Implication} (" +
                    $"(DiceOne(x) {AND} {AllQuantum}y (NotEquals(x,y) {Implication} NotSelected(y)))" +
                    $"{OR} {ExistQuantum}y (DiceTwo(x) {AND} DiceTwo(y) {AND} NotEquals(x,y) {AND} {AllQuantum}u ((NotEquals(x,u) {AND} NotEquals(y,u)) {Implication} NotSelected(u)))" +
                    $"{OR} (DiceThree(x) {AND} {AllQuantum}y (NotEquals(x,y) {Implication} NotSelected(y)))" +
                    $"{OR} {ExistQuantum}y (DiceThree(x) {AND} DiceThree(y) {AND} NotEquals(x,y) {AND} {AllQuantum}u ((NotEquals(x,u) {AND} NotEquals(y,u)) {Implication} NotSelected(u)))" +
                    $"{OR} {ExistQuantum}y {ExistQuantum}z (DiceThree(x) {AND} DiceThree(y) {AND} DiceThree(z) {AND} NotEquals(x,y) {AND} NotEquals(x,z) {AND} NotEquals(y,z) {AND} {AllQuantum}u ((NotEquals(x,u) {AND} NotEquals(y,u) {AND} NotEquals(z,u)) {Implication} NotSelected(u)))" +
                    $"{OR} {ExistQuantum}y (DiceFour(x) {AND} DiceFour(y) {AND} NotEquals(x,y) {AND} {AllQuantum}u ((NotEquals(x,u) {AND} NotEquals(y,u)) {Implication} NotSelected(u)))" +
                    $"{OR} {ExistQuantum}y {ExistQuantum}z (DiceFour(x) {AND} DiceFour(y) {AND} DiceFour(z) {AND} NotEquals(x,y) {AND} NotEquals(x,z) {AND} NotEquals(y,z) {AND} {AllQuantum}u ((NotEquals(x,u) {AND} NotEquals(y,u) {AND} NotEquals(z,u)) {Implication} NotSelected(u)))" +
                    $"{OR} {ExistQuantum}y (DiceFive(x) {AND} DiceFive(y) {AND} NotEquals(x,y) {AND} {AllQuantum}u ((NotEquals(x,u) {AND} NotEquals(y,u)) {Implication} NotSelected(u)))" +
                    $"{OR} {ExistQuantum}y {ExistQuantum}z (DiceFive(x) {AND} DiceFive(y) {AND} DiceFive(z) {AND} NotEquals(x,y) {AND} NotEquals(x,z) {AND} NotEquals(y,z) {AND} {AllQuantum}u ((NotEquals(x,u) {AND} NotEquals(y,u) {AND} NotEquals(z,u)) {Implication} NotSelected(u)))" +
                    $"{OR} (DiceSix(x) {AND} {AllQuantum}y (NotEquals(x,y) {Implication} NotSelected(y)))))"
                };
            case ERule.Dice1x1:
                return new List<string>
                {
                    $"{ExistQuantum}x (Selected(x) {AND} DiceOne(x)" +
                    $"{AND} {AllQuantum}u (NotEquals(u,x) {Implication} NotSelected(u)))"
                };
            case ERule.Dice2x2:
                return new List<string>
                {
                    $"{ExistQuantum}x {ExistQuantum}y (Selected(x) {AND} Selected(y) {AND} NotEquals(x,y) {AND} DiceTwo(x) {AND} DiceTwo(y)" +
                    $"{AND} {AllQuantum}u ((NotEquals(u,x) {AND} NotEquals(u,y)) {Implication} NotSelected(u)))"
                };
            case ERule.Dice1x3:
                return new List<string>
                {
                    $"{ExistQuantum}x (Selected(x) {AND} DiceThree(x)" +
                    $"{AND} {AllQuantum}u (NotEquals(u,x) {Implication} NotSelected(u)))"
                };
            case ERule.Dice2x3:
                return new List<string>
                {
                    $"{ExistQuantum}x {ExistQuantum}y (Selected(x) {AND} Selected(y) {AND} NotEquals(x,y) {AND} DiceThree(x) {AND} DiceThree(y)" +
                    $"{AND} {AllQuantum}u ((NotEquals(u,x) {AND} NotEquals(u,y)) {Implication} NotSelected(u)))"
                };
            case ERule.Dice3x3:
                return new List<string>
                {
                    $"{ExistQuantum}x {ExistQuantum}y {ExistQuantum}z (Selected(x) {AND} Selected(y) {AND} Selected(z) {AND} NotEquals(x,y) {AND} NotEquals(x,z) {AND} DiceThree(x) {AND} DiceThree(y) {AND} DiceThree(z)" +
                    $"{AND} {AllQuantum}u ((NotEquals(u,x) {AND} NotEquals(u,y) {AND} NotEquals(u,z)) {Implication} NotSelected(u)))"
                };
            case ERule.Dice2x4:
                return new List<string>
                {
                    $"{ExistQuantum}x {ExistQuantum}y (Selected(x) {AND} Selected(y) {AND} NotEquals(x,y) {AND} DiceFour(x) {AND} DiceFour(y)" +
                    $"{AND} {AllQuantum}u ((NotEquals(u,x) {AND} NotEquals(u,y)) {Implication} NotSelected(u)))"
                };
            case ERule.Dice3x4:
                return new List<string>
                {
                    $"{ExistQuantum}x {ExistQuantum}y {ExistQuantum}z (Selected(x) {AND} Selected(y) {AND} Selected(z) {AND} NotEquals(x,y) {AND} NotEquals(x,z) {AND} DiceFour(x) {AND} DiceFour(y) {AND} DiceFour(z)" +
                    $"{AND} {AllQuantum}u ((NotEquals(u,x) {AND} NotEquals(u,y) {AND} NotEquals(u,z)) {Implication} NotSelected(u)))"
                };
            case ERule.Dice2x5:
                return new List<string>
                {
                    $"{ExistQuantum}x {ExistQuantum}y (Selected(x) {AND} Selected(y) {AND} NotEquals(x,y) {AND} DiceFive(x) {AND} DiceFive(y)" +
                    $"{AND} {AllQuantum}u ((NotEquals(u,x) {AND} NotEquals(u,y)) {Implication} NotSelected(u)))"
                };
            case ERule.Dice3x5:
                return new List<string>
                {
                    $"{ExistQuantum}x {ExistQuantum}y {ExistQuantum}z (Selected(x) {AND} Selected(y) {AND} Selected(z) {AND} NotEquals(x,y) {AND} NotEquals(x,z) {AND} NotEquals(y,z) {AND} DiceFive(x) {AND} DiceFive(y) {AND} DiceFive(z)" +
                    $"{AND} {AllQuantum}u ((NotEquals(u,x) {AND} NotEquals(u,y) {AND} NotEquals(u,z)) {Implication} NotSelected(u)))"
                };
            case ERule.Dice1x6:
                return new List<string>
                {
                    $"{ExistQuantum}x (Selected(x) {AND} DiceSix(x)" +
                    $"{AND} {AllQuantum}u (NotEquals(u,x) {Implication} NotSelected(u)))"
                };
            default:
                return new List<string>() { };
        }
    }

    public enum ERule
    {
        Dice1x1,
        Dice2x2,
        Dice1x3,
        Dice2x3,
        Dice3x3,
        Dice2x4,
        Dice3x4,
        Dice2x5,
        Dice3x5,
        Dice1x6,
        SelectionPossible
    }
}
