using System;
using System.Linq.Expressions;
using ExpressionBuilder.Methods;

namespace ExpressionBuilder;

public delegate Expression<Func<TSource, bool>> CollectionDelegate<TSource, TItem>(CollectionOptions<TSource, TItem> options);
