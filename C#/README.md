# Common
A set of helper and extension classes that I commonly roll into applications

## CircularBuffer&lt;T&gt;

A [circular buffer](https://en.wikipedia.org/wiki/Circular_buffer "Circular Buffer") implementation for C#.

* The buffer can be initialised with OverWriting either enabled or
disabled (disabled by default). If it is enabled, an exception
will be thrown if the buffer capacity is exceeded, otherwise it
will simply drop as many elements off the back as needed in order
to stay within the capacity limit.

* Data is added singularly with Add() and removed singularly with
Remove(). The usual ICollection&lt;T&gt; methods all work as expected.

* Read() and Write() have been provided to extract or add blocks
of data respectively. These are most useful when reading or writing
into the buffer from a stream, for example.

## NotifyBase

A base class inherited by WPF view models. It implements ```INotifyPropertyChanged```
and provides a few convenience methods for change notification.

A simple Command base class is also provided. 
