using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubscriptionManager {
	private Dictionary<IPublisher, List<ISubscriber>> subscriptions = new Dictionary<IPublisher, List<ISubscriber>>();

	public void notifySubscribers(IPublisher publisher) {
		List<ISubscriber> subscribers = subscriptions[publisher];
		foreach (ISubscriber subscriber in subscribers) {
			subscriber.notify();
		}
	}
	public void registerPublisher(IPublisher publisher) {
		subscriptions[publisher] = new List<ISubscriber>();
	}
	public void subscribe(ISubscriber subscriber, IPublisher publisher) {
		subscriptions[publisher].Add(subscriber);
	}
}