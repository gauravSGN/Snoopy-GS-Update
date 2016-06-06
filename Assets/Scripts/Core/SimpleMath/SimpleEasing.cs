/*
 * Ported to C# from jQuery Easing v1.3 - http://gsgd.co.uk/sandbox/jquery/easing/
 * 
 * Original jQuery copyright
 * Copyright Â© 2008 George McGinley Smith
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of 
 * conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list 
 * of conditions and the following disclaimer in the documentation and/or other materials 
 * provided with the distribution.
 * 
 * Neither the name of the author nor the names of contributors may be used to endorse 
 * or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
 *  COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 *  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
 *  GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED 
 * AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 *  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED 
 * OF THE POSSIBILITY OF SUCH DAMAGE. 
 *
*/

using UnityEngine;
using System.Collections;

public class SimpleEasing : MonoBehaviour {
//t: current time, b: begInnIng value, c: change In value, d: duration
	private const float PI2 = 6.283185307179586f;
	private const float Asin1 = 1.5707963267948966f;

	public static float Linear(float currTime, float duration, float startVal, float endVal){
		float delta = endVal - startVal;
		float perc = currTime / duration;
		return startVal + (perc * delta);
	}

	public static float EaseInQuad(float currTime, float duration, float startVal, float endVal){
		float change = endVal - startVal;
		currTime /= duration;
		return change*currTime*currTime + startVal;
	}

	public static float EaseOutQuad(float currTime, float duration, float startVal, float endVal){
		float change = endVal - startVal;
		currTime /= duration;
		return -change * currTime * (currTime - 2) + startVal;
	}

	public static float EaseInOutQuad(float currTime, float duration, float startVal, float endVal){
		float change = endVal - startVal;
		currTime = currTime / (duration / 2);
		if (currTime < 1){ return (change / 2) * Mathf.Pow(currTime, 2) + startVal;}
		currTime--;
		return -change / 2 * (currTime * (currTime - 2) - 1) + startVal;
	}

	public static float EaseInCubic(float currTime, float duration, float startVal, float endVal){
		float change = endVal - startVal;
		currTime /= duration;
		return change * currTime * currTime * currTime + startVal;
	}

	public static float EaseOutCubic(float currTime, float duration, float startVal, float endVal){
		float change = endVal - startVal;
		currTime /= duration;
		currTime--;
		return change * (Mathf.Pow(currTime, 3) + 1) + startVal;
	}

	public static float EaseInOutCubic(float currTime, float duration, float startVal, float endVal){
		float retVal;
		float change = endVal - startVal;
		currTime = currTime / (duration / 2);
		if (currTime < 1){
			retVal = (change/2) * Mathf.Pow(currTime, 3) + startVal;
		}
		else{
			currTime -= 2;
			retVal = (change / 2) * (Mathf.Pow(currTime, 3) + 2) + startVal;
		}
		return retVal;
	}

	public static float EaseInQuart(float currTime, float duration, float startVal, float endVal){
		float change = endVal - startVal;
		currTime /= duration;
		return change * Mathf.Pow(currTime, 4) + startVal;
	}

	public static float EaseOutQuart(float currTime, float duration, float startVal, float endVal){
		float change = endVal - startVal;
		currTime /= duration;
		currTime--;
		return -change * (Mathf.Pow(currTime, 4) - 1) + startVal;
	}

	public static float EaseInOutQuart(float currTime, float duration, float startVal, float endVal){
		float retVal;
		float change = endVal - startVal;
		currTime = currTime / (duration / 2);
		if (currTime < 1){ 
			retVal = (change / 2)*Mathf.Pow(currTime, 4) + startVal;
		}
		else{
			currTime -= 2;
			retVal = -change/2 * (Mathf.Pow(currTime, 4) - 2) + startVal;
		}
		return retVal;
	}

	public static float EaseInQuint(float currTime, float duration, float startVal, float endVal){
		currTime /= duration;
		float change = endVal - startVal;
		return change*Mathf.Pow(currTime, 5) + startVal;
	}

	public static float EaseOutQuint(float currTime, float duration, float startVal, float endVal){
		currTime /= duration;
		currTime--;
		float change = endVal - startVal;
		return change * (Mathf.Pow(currTime, 5) + 1) + startVal;
	}

	public static float EaseInOutQuint(float currTime, float duration, float startVal, float endVal){
		float retVal;
		currTime = currTime / (duration / 2);
		float change = endVal - startVal;
		if (currTime < 1){
			retVal =  (change / 2) * Mathf.Pow(currTime, 5) + startVal;
		}
		else{
			currTime -= 2;
			retVal = (change / 2) * (Mathf.Pow(currTime, 5) + 2) + startVal;
		}
		return retVal;
	}

	public static float EaseInSine(float currTime, float duration, float startVal, float endVal){
		float change = endVal - startVal;
		return -change * Mathf.Cos(currTime / duration * (Mathf.PI/2)) + change + startVal;
	}

	public static float EaseOutSine(float currTime, float duration, float startVal, float endVal){
		float change = endVal - startVal;
		return change * Mathf.Sin(currTime / duration * (Mathf.PI/2)) + startVal;
	}

	public static float EaseInOutSine(float currTime, float duration, float startVal, float endVal){
		float change = endVal - startVal;
		return -change/2 * (Mathf.Cos(Mathf.PI*currTime / duration) - 1) + startVal;
	}

	public static float EaseInExpo(float currTime, float duration, float startVal, float endVal){
		if(currTime == 0){
			return startVal;
		}
		float change = endVal - startVal;
		return (currTime==0) ? startVal : change * Mathf.Pow(2, 10 * (currTime / duration - 1)) + startVal;
	}

	public static float EaseOutExpo(float currTime, float duration, float startVal, float endVal) {
		if(currTime == duration){
			return endVal;
		}
		float change = endVal - startVal;
		return startVal + change * (1 - Mathf.Pow(2, -10 * currTime/duration));
	}

	public static float EaseInOutExpo(float currTime, float duration, float startVal, float endVal) {
		if(currTime == 0){ return startVal; }
		if(currTime == duration){ return endVal; }
		float retVal;
		float change = endVal - startVal;
		currTime = currTime / (duration / 2);
		if (currTime < 1){
			retVal = (change / 2) * Mathf.Pow(2, 10 * (currTime - 1) ) + startVal;
		}
		else{
			currTime--;
			retVal = (change / 2) * ( -Mathf.Pow(2, -10 * currTime) + 2 ) + startVal;
		}
		return retVal;
	}

	public static float EaseInCirc(float currTime, float duration, float startVal, float endVal) {
		float change = endVal - startVal;
		currTime /= duration;
		return startVal - (change * (Mathf.Sqrt(1 - currTime * currTime) - 1));
	}

	public static float EaseOutCirc(float currTime, float duration, float startVal, float endVal) {
		float change = endVal - startVal;
		currTime /= duration;
		currTime--;
		return startVal + change * Mathf.Sqrt(1 - currTime*currTime);
	}

	public static float EaseInOutCirc(float currTime, float duration, float startVal, float endVal) {
		float retVal;
		float change = endVal - startVal;
		currTime = currTime / (duration / 2);
		if (currTime < 1){
			retVal = startVal - change/2 * (Mathf.Sqrt(1 - currTime * currTime) - 1);
		}
		else{
			currTime -= 2;
			retVal = startVal + change/2 * (Mathf.Sqrt(1 - currTime * currTime) + 1);
		}
		return retVal;
	}

	public static float EaseInElastic(float currTime, float duration, float startVal, float endVal){
		if (currTime == 0){return startVal;}
		if (currTime == duration){return endVal;}
		currTime /= duration;
		float change = endVal - startVal;
		float p = duration*0.3f;
		float s = change < 0 ?  (p / 4) : ((p / PI2) * Asin1);
		currTime--;
		return startVal - (change * Mathf.Pow(2,10 * currTime) * Mathf.Sin((currTime * duration - s) * PI2 / p));
	}

	public static float EaseOutElastic(float currTime, float duration, float startVal, float endVal){
		if (currTime == 0){ return startVal; }
		if (currTime == duration){ return endVal; }
		currTime /= duration;
		float change = endVal-startVal;
		float p=duration*0.3f;
		float s= change < 0 ? (p / 4) : ((p / PI2) * Asin1);
		return startVal + change + (change * Mathf.Pow(2,-10 * currTime) * Mathf.Sin((currTime * duration - s) * PI2 / p));
	}

	public static float EaseInOutElastic(float currTime, float duration, float startVal, float endVal){
		if (currTime == 0) return startVal;
		if ((currTime /= (duration / 2)) == 2) return endVal;
		float change = endVal - startVal;
		float p = duration * (0.3f * 1.5f);
		float s = change < 0 ? (p/4) : (p/(PI2) * Asin1);
		float retVal;
		if (currTime < 1){
			currTime--;
			retVal = startVal - (0.5f * (change * Mathf.Pow(2,10 * currTime) * Mathf.Sin((currTime * duration - s) * PI2 / p)));
		}
		else{
			currTime--;
			retVal = startVal + change + (0.5f * change * Mathf.Pow(2,-10 * currTime) * Mathf.Sin((currTime * duration - s) * PI2 / p));
		}
		return retVal;
	}

	public static float EaseInBack(float currTime, float duration, float startVal, float endVal, float s = 1.70158f){
		float perc = currTime / duration;
		float change = endVal - startVal;
		return startVal + change * (perc * perc * ((s + 1) * perc - s));
	}

	public static float EaseOutBack(float currTime, float duration, float startVal, float endVal, float s = 1.70158f){
		float perc = (currTime / duration) - 1;
		float change = endVal - startVal;
		return  startVal + (change * (perc * perc * ((s + 1) * perc + s) + 1));
	}

	public static float EaseInOutBack(float currTime, float duration, float startVal, float endVal, float s = 1.70158f){
		float retVal;
		float perc = currTime / (duration / 2);
		float change = endVal - startVal;
		s *= 1.525f;
		if (perc < 1){
			retVal = startVal + (change / 2 * (perc * perc * ((s + 1) * perc - s)));
		}
		else{
			perc -= 2;
			retVal = startVal + (change / 2 * (perc * perc * ((s + 1) * perc + s) + 2));
		}
		return retVal;
	}

	public static float EaseInBounce(float currTime, float duration, float startVal, float endVal){
		float change = endVal - startVal;
		return change - EaseOutBounce(duration - currTime, duration, 0, endVal) + startVal;
	}

	public static float EaseOutBounce(float currTime, float duration, float startVal, float endVal){
		float retVal;
		float change = endVal - startVal;
		float perc = currTime / duration;
		if (perc < (1f/2.75f)) {
			retVal = change*(7.5625f*perc*perc) + startVal;
		}
		else if (perc < (2f/2.75f)) {
			perc -= (1.5f/2.75f);
			retVal = change*(7.5625f*perc*perc + 0.75f) + startVal;
		}
		else if (perc < (2.5f/2.75f)) {
			perc -= (2.25f/2.75f);
			retVal = change*(7.5625f*perc*perc + 0.9375f) + startVal;
		}
		else {
			perc -= (2.625f / 2.75f);
			retVal = change*(7.5625f*perc*perc + 0.984375f) + startVal;
		}
		return retVal;
	}

	public static float EaseInOutBounce(float currTime, float duration, float startVal, float endVal){
		float retVal;
		if (currTime < duration/2){
			retVal = EaseInBounce(currTime*2, duration, 0, endVal) * 0.5f + startVal;
		}
		else{
			float change = endVal - startVal;
			retVal = EaseOutBounce(currTime*2-duration, duration, 0, endVal) * 0.5f + change*0.5f + startVal;
		}
		return retVal;
	}

}
