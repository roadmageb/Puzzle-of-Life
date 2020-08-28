using System;

public enum Cell { NULL, EMPTY, ANY, CELL1, CELL2, CELL3, TARGET1, TARGET2, TARGET3 }
public enum ConstraintType { GE, LE, EQ, NE, BET }
public enum PlayState { EDIT, EDITTOINIT, PLAY, PLAYFRAME, ERROR }
public enum ButtonState { PLAY, FASTFORWARD, PAUSE, PLAYFRAME, RESETGRAY, RESETRED, STOP }
public enum ConstraintState { NORMAL, DUMMY, SELTYPE, SELNUM }
public enum EditorButtonType { ADD, RESET, DELETE, CONSTTYPE, CONSTNUM }
public enum BoardState { MENUBLACK, MENUYELLOW, NEXTLEVELNOTABLE, NEXTLEVELBLACK, NEXTLEVELGREEN }
