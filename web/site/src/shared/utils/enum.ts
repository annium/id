export function getKey<T>(src: Record<string | number, T>, value: T) {
  return Object.keys(src).find(x => src[x] === value)
}

export function getPairs<T>(src: Record<string | number, T>) {
  return Object.keys(src).filter(x => isNaN(Number(x))).map(x => [x, src[x]])
}
