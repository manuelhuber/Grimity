using System;

namespace Grimity.Data {
public static class ObservableExtensions {
    public static void Add(this Observable<double> observable, double amount) {
        observable.Set(curr => curr + amount);
    }

    public static void Subtract(this Observable<double> observable, double amount) {
        observable.Set(curr => curr - amount);
    }

    public static void Add(this Observable<int> observable, int amount) {
        observable.Set(curr => curr + amount);
    }

    public static void Subtract(this Observable<int> observable, int amount) {
        observable.Set(curr => curr - amount);
    }

    public static void Add(this Observable<long> observable, long amount) {
        observable.Set(curr => curr + amount);
    }

    public static void Subtract(this Observable<long> observable, long amount) {
        observable.Set(curr => curr - amount);
    }

    public static void Add(this Observable<ulong> observable, ulong amount) {
        observable.Set(curr => curr + amount);
    }

    public static void Subtract(this Observable<ulong> observable, ulong amount) {
        observable.Set(curr => curr - amount);
    }

    public static IObservable<T> Map<TSource, T>(this IObservable<TSource> source, Func<TSource, T> map) {
        return new MappedObservable<TSource, T>(source, map);
    }
}
}