using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiderProgram : MonoBehaviour {
	private List<Action> program = new List<Action>(8);
	private Actor spider;
	private int currentIndex = 0;

	public enum Action { forward, left, right, back, fire, updown };

	// ------------------------------------------------------------------------

	void Awake() {
		spider = gameObject.GetComponent<Actor>();
	}

	// ------------------------------------------------------------------------

	public void start() {
		StartCoroutine("SpiderProgram_" + gameObject.name, run());
	}

	// ------------------------------------------------------------------------

	public void stop() {
		StopCoroutine("SpiderProgram_" + gameObject.name);
	}

	// ------------------------------------------------------------------------

	public void clear() {
		program.Clear();
	}

	// ------------------------------------------------------------------------

	public void add(Action a) {
		program.Add(a);
	}

	// ------------------------------------------------------------------------

	public void delete() {
		if(program.Count > 0)
			program.RemoveAt(program.Count - 1);
	}

	// ------------------------------------------------------------------------

	public void delete(int index) {
		if(program.Count > 0 && index >=0 && index < program.Count)
			program.RemoveAt(index);
	}

	// ------------------------------------------------------------------------

	private IEnumerator run() {
		for(currentIndex = 0; currentIndex < program.Count; currentIndex++) {
			Action a = program[currentIndex];
		//	if(a == Action.)
		//	if(!actor.)
			yield return new WaitForSeconds(0.05f);
		}
		yield return null;
	}
}