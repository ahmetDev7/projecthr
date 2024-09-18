# API Endpoints Documentation


## Base URL

- **Base URL**: `http://localhost:3000/api/v1`


## Warehouses

- `GET /warehouses` - Get all warehouses.

- `GET /warehouses/{warehouse_id}` - Get a specific warehouse by ID.

- `GET /warehouses/{warehouse_id}/locations` - Get all locations for a specific warehouse.

- `POST /warehouses` - Create a new warehouse.

- `PUT /warehouses/{warehouse_id}` - Update a specific warehouse by ID.

- `DELETE /warehouses/{warehouse_id}` - Delete a specific warehouse by ID.


## Locations

- `GET /locations` - Get all locations.

- `GET /locations/{location_id}` - Get a specific location by ID.

- `POST /locations` - Create a new location.

- `PUT /locations/{location_id}` - Update a specific location by ID.

- `DELETE /locations/{location_id}` - Delete a specific location by ID.



## Transfers

- `GET /transfers` - Get all transfers.

- `GET /transfers/{transfer_id}` - Get a specific transfer by ID.

- `GET /transfers/{transfer_id}/items` - Get all items for a specific transfer.

- `POST /transfers` - Create a new transfer.

- `PUT /transfers/{transfer_id}` - Update a specific transfer by ID.

- `PUT /transfers/{transfer_id}/commit` - Commit a specific transfer.

- `DELETE /transfers/{transfer_id}` - Delete a specific transfer by ID.


## Items

- `GET /items` - Get all items.

- `GET /items/{item_id}` - Get a specific item by ID.

- `GET /items/{item_id}/inventory` - Get all inventories for a specific item.

- `GET /items/{item_id}/inventory/totals` - Get inventory totals for a specific item.

- `POST /items` - Create a new item.

- `PUT /items/{item_id}` - Update a specific item by ID.

- `DELETE /items/{item_id}` - Delete a specific item by ID.


## Item Lines

- `GET /item_lines` - Get all item lines.

- `GET /item_lines/{item_line_id}` - Get a specific item line by ID.

- `GET /item_lines/{item_line_id}/items` - Get all items for a specific item line.

- `PUT /item_lines/{item_line_id}` - Update a specific item line by ID.

- `DELETE /item_lines/{item_line_id}` - Delete a specific item line by ID.





## Item Groups

- `GET /item_groups` - Get all item groups.

- `GET /item_groups/{item_group_id}` - Get a specific item group by ID.

- `GET /item_groups/{item_group_id}/items` - Get all items for a specific item group.

- `PUT /item_groups/{item_group_id}` - Update a specific item group by ID.

- `DELETE /item_groups/{item_group_id}` - Delete a specific item group by ID.





## Item Types

- `GET /item_types` - Get all item types.

- `GET /item_types/{item_type_id}` - Get a specific item type by ID.

- `GET /item_types/{item_type_id}/items` - Get all items for a specific item type.

- `PUT /item_types/{item_type_id}` - Update a specific item type by ID.

- `DELETE /item_types/{item_type_id}` - Delete a specific item type by ID.





### Inventories

- `GET /inventories` - Get all inventories.

- `GET /inventories/{inventory_id}` - Get a specific inventory by ID.

- `POST /inventories` - Create a new inventory.

- `PUT /inventories/{inventory_id}` - Update a specific inventory by ID.

- `DELETE /inventories/{inventory_id}` - Delete a specific inventory by ID.






## Suppliers

- `GET /suppliers` - Get all suppliers.

- `GET /suppliers/{supplier_id}` - Get a specific supplier by ID.

- `GET /suppliers/{supplier_id}/items` - Get all items for a specific supplier.

- `POST /suppliers` - Create a new supplier.

- `PUT /suppliers/{supplier_id}` - Update a specific supplier by ID.

- `DELETE /suppliers/{supplier_id}` - Delete a specific supplier by ID.






## Orders

- `GET /orders` - Get all orders.

- `GET /orders/{order_id}` - Get a specific order by ID.

- `GET /orders/{order_id}/items` - Get all items for a specific order.

- `POST /orders` - Create a new order.

- `PUT /orders/{order_id}` - Update a specific order by ID.

- `PUT /orders/{order_id}/items` - Update items in a specific order.

- `DELETE /orders/{order_id}` - Delete a specific order by ID.




## Clients

- `GET /clients` - Get all clients.

- `GET /clients/{client_id}` - Get a specific client by ID.

- `GET /clients/{client_id}/orders` - Get all orders for a specific client.

- `POST /clients` - Create a new client.

- `PUT /clients/{client_id}` - Update a specific client by ID.

- `DELETE /clients/{client_id}` - Delete a specific client by ID.






## Shipments

- `GET /shipments` - Get all shipments.

- `GET /shipments/{shipment_id}` - Get a specific shipment by ID.

- `GET /shipments/{shipment_id}/orders` - Get all orders for a specific shipment.

- `GET /shipments/{shipment_id}/items` - Get all items for a specific shipment.

- `POST /shipments` - Create a new shipment.

- `PUT /shipments/{shipment_id}` - Update a specific shipment by ID.

- `PUT /shipments/{shipment_id}/orders` - Update orders in a specific shipment.

- `PUT /shipments/{shipment_id}/items` - Update items in a specific shipment.

- `DELETE /shipments/{shipment_id}` - Delete a specific shipment by ID.