#pragma strict

var onGround : boolean;

function Start () {
	onGround = false;
}

function Update () {

}

function OnTriggerEnter()
{
	onGround = true;
}

function OnTriggerExit()
 {
 	onGround = false;
 }