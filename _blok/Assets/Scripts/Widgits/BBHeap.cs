using UnityEngine;
using System.Collections;
using System;

//Heap Structure used for optimization

public class BBHeap <T> where T: BBIHeapItem<T> {
	T[] items;
	int currentItemCount;
	
	public BBHeap(int maxHeapSize) {
		items = new T[maxHeapSize];
	}
	
	public void Add(T item) {
		item.HeapIndex = currentItemCount;
		items[currentItemCount] = item;
		this.SortUp(item);
		currentItemCount++;
	}
	
	public T RemoveFirst() {
		T firstItem = items[0];
		currentItemCount--;
		items[0] = items[currentItemCount];
		items[0].HeapIndex = 0;
		SortDown(items[0]);
		return firstItem;
	}
	
	public void UpdateItem(T item) {
		this.SortUp(item);
	}
	
	public int Count {
		get { return this.currentItemCount; }
	}
	
	public bool Contains(T item) {
		return Equals(items[item.HeapIndex], item);
	}
	
	private void SortDown(T item) {
		while (true) {
			int childIndexLeft = item.HeapIndex * 2 + 1;
			int childIndexRight = item.HeapIndex * 2 + 2;
			int swapIndex = 0;
			//If within heap constraints
			if (childIndexLeft < currentItemCount) {
				swapIndex = childIndexLeft;
				//If a right child exists
				if (childIndexRight < currentItemCount) {
					if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
						swapIndex = childIndexRight;
					}
				}
				
				if (item.CompareTo(items[swapIndex]) < 0) {
					Swap(item, items[swapIndex]);
				} else {
					return;
				}
			} else {
				return;
			}
		}
	}
	
	private void SortUp(T item) {
		int parentIndex = (item.HeapIndex - 1) / 2;
		while (true) {
			T parentItem = items[parentIndex];
			if (item.CompareTo(parentItem) > 0) {
				this.Swap(item, parentItem);
			} else {
				break;
			}
			parentIndex = (item.HeapIndex - 1) / 2;
		}
	}
	
	private void Swap(T a, T b) {
		items[a.HeapIndex] = b;
		items[b.HeapIndex] = a;
		int aIndex = a.HeapIndex;
		a.HeapIndex = b.HeapIndex;
		b.HeapIndex = aIndex;
	}
	
}

public interface BBIHeapItem<T> : IComparable<T> {
	int HeapIndex {
		get; set;
	}
}
