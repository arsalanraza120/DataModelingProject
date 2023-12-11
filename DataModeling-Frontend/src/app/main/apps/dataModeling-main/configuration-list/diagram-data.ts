

export const nodes = [
  { id: 'Customer', label: 'Customer' },
  { id: 'Order', label: 'Order' },
  { id: 'Product', label: 'Product' },
];

export const links = [
  { source: 'Customer', target: 'Order', label: 'Places' },
  { source: 'Order', target: 'Product', label: 'Contains' },
];
  