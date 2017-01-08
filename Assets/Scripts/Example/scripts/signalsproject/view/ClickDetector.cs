/// Uses a signal instead of an EventDispatcher

using System;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using strange.extensions.command.impl;
using strange.extensions.context.api;

namespace strange.examples.signals
{
	public class ClickDetector : View
	{
		// Note how we're using a signal now
		public Signal clickSignal = new Signal();

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView { get; set; }
		
		void OnMouseDown()
		{
            Debug.LogError("---------------ClickDetector view --------------");
			clickSignal.Dispatch(); 
		}
	}
}

