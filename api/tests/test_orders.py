import unittest
from httpx import Client
from httpx import codes

# 7 test integrations happy path
# 5 test integrations edge cases

class TestOrders(unittest.TestCase):

    def setUp(self):
        self.client = Client(base_url="http://localhost:3000/api/v1/", headers={"Content-Type": "application/json", "API_KEY": "a1b2c3d4e5"})

    def set_random_key(self):
        self.client.headers["API_KEY"] = "xd"

    def set_reader_key(self):
        self.client.headers["API_KEY"] = "f6g7h8i9j0"

    def test_get_all(self):
        response = self.client.get("/orders")

        self.assertEqual(response.status_code, codes.OK)
        self.assertTrue(len(response.json()) > 0)

    def test_get_all_unauthorized(self):
        self.set_random_key()
        response = self.client.get("/orders")

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)

    def test_get_all_reader(self):
        self.set_reader_key()
        response = self.client.get("/orders")

        self.assertEqual(response.status_code, codes.OK)
        
    def test_get_single(self):
        response = self.client.get("/orders/1")

        self.assertEqual(response.status_code, codes.OK)
        self.assertIn("id", response.json())

    def test_get_single_unauthorized(self):
        self.set_random_key()
        response = self.client.get("/orders/1")

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)
    
    def test_get_single_reader(self):
        self.set_reader_key()
        response = self.client.get("/orders/1")

        self.assertEqual(response.status_code, codes.OK)
    
    def test_create_order(self):
        new_order = {
            "id": 4000,
            "source_id": 48,
            "order_date": "2016-04-10T09:57:53Z",
            "request_date": "2016-04-14T09:57:53Z",
            "reference": "ORD02785",
            "reference_extra": "Tekenen falen verdrietig vergeven voeden oorlog.",
            "order_status": "Shipped",
            "notes": "Veer eenzaam boom bloed waarschijnlijk.",
            "shipping_notes": "Lang smal meest ziel.",
            "picking_notes": "Verrassen vriezen elektrisch kom zo.",
            "warehouse_id": 43,
            "ship_to": 1041,
            "bill_to": 1028,
            "shipment_id": 8243,
            "total_amount": 4642.86,
            "total_discount": 350.94,
            "total_tax": 291.88,
            "total_surcharge": 32.99,
            "created_at": "2016-04-10T09:57:53Z",
            "updated_at": "2016-04-12T05:57:53Z",
            "items": [
                {
                    "item_id": "P005235",
                    "amount": 34
                },
                {
                    "item_id": "P004381",
                    "amount": 4
                },
                {
                    "item_id": "P002213",
                    "amount": 14
                }
            ]
        }
        
        response = self.client.post("/orders", json=new_order)
        self.assertEqual(response.status_code, codes.CREATED)  # HTTP 201 Created

    def test_create_order_unauthorized(self):
        self.set_random_key()
        new_order = {
            "id": 4000,
            "source_id": 48,
            "order_date": "2016-04-10T09:57:53Z",
            "request_date": "2016-04-14T09:57:53Z",
            "reference": "ORD02785",
            "reference_extra": "Tekenen falen verdrietig vergeven voeden oorlog.",
            "order_status": "Shipped",
            "notes": "Veer eenzaam boom bloed waarschijnlijk.",
            "shipping_notes": "Lang smal meest ziel.",
            "picking_notes": "Verrassen vriezen elektrisch kom zo.",
            "warehouse_id": 43,
            "ship_to": 1041,
            "bill_to": 1028,
            "shipment_id": 8243,
            "total_amount": 4642.86,
            "total_discount": 350.94,
            "total_tax": 291.88,
            "total_surcharge": 32.99,
            "created_at": "2016-04-10T09:57:53Z",
            "updated_at": "2016-04-12T05:57:53Z",
            "items": [
                {
                    "item_id": "P005235",
                    "amount": 34
                },
                {
                    "item_id": "P004381",
                    "amount": 4
                },
                {
                    "item_id": "P002213",
                    "amount": 14
                }
            ]
        }
        
        response = self.client.post("/orders", json=new_order)
        self.assertEqual(response.status_code, codes.UNAUTHORIZED)  # HTTP 401 UNAUTHORIZED

    def test_update_order(self):
        updated_order = {
            "ship_to": 1045,
            "bill_to": 1035,
        }
        response = self.client.put("/orders/4000", json=updated_order)

        self.assertEqual(response.status_code, codes.OK) # HTTP 200 OK
    
    def test_update_order_unauthorized(self):
        self.set_random_key()
        
        updated_order = {
            "ship_to": 1045,
            "bill_to": 1035,
        }
        response = self.client.put("/orders/2", json=updated_order)

        self.assertEqual(response.status_code, codes.UNAUTHORIZED) # HTTP 401 UNAUTHORIZED

    def test_delete_order(self):
        response = self.client.delete("/orders/2")

        self.assertEqual(response.status_code, codes.OK)  # HTTP 200 OK

    def test_delete_order_unauthorized(self):
        self.set_random_key()
        response = self.client.delete("/orders/2")

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)  # HTTP 401 UNAUTHORIZED


    
