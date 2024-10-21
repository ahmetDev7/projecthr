import unittest
from httpx import Client
from httpx import codes

# 7 happy paths
# 10 edge cases


class TestItems(unittest.TestCase):

    def setUp(self):
        self.reader_token = "f6g7h8i9j0"
        self.client = Client(base_url="http://localhost:3000/api/v1/",
                             headers={"Content-Type": "application/json", "API_KEY": "a1b2c3d4e5"})
    
    def test_readertoken_get_all(self):
        self.client.headers["API_KEY"] = self.reader_token
        response = self.client.get("/items")

        self.assertEqual(response.status_code, codes.OK)
    
    def test_readertoken_get_single(self):
        self.client.headers["API_KEY"] = self.reader_token
        response = self.client.get("/items/P000001")

        self.assertEqual(response.status_code, codes.OK)
        
    def test_unauthorized_get_all(self):
        self.client.headers["API_KEY"] = "wrong_key"
        response = self.client.get("/items")

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)

    def test_get_single_unauthorized(self):
        self.client.headers["API_KEY"] = "wrong_key"
        response = self.client.get("/items/P000001")

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)

    def test_get_incorrect_id_unauthorized(self):
        self.client.headers["API_KEY"] = "wrong_key"
        response = self.client.get("/items/AP000001")

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)

    def test_create_client_unauthorized(self):
        self.client.headers["API_KEY"] = "wrong_key"
        new_client = {
            "uid": "P000001",
            "code": "sjQ24508K",
            "description": "BABA SHAMPOO 500ML",
            "short_description": "must",
            "upc_code": "6523540947122",
            "model_number": "63-OFFTq0T",
            "commodity_code": "oTo304",
            "item_line": 11,
            "item_group": 73,
            "item_type": 14,
            "unit_purchase_quantity": 47,
            "unit_order_quantity": 13,
            "pack_order_quantity": 11,
            "supplier_id": 34,
            "supplier_code": "SUP423",
            "supplier_part_number": "E-86805-uTM",
            "created_at": "2015-02-19 16:08:24",
            "updated_at": "2015-09-26 06:37:56"

        }

        response = self.client.post("/items", json=new_client)
        self.assertEqual(response.status_code, codes.UNAUTHORIZED)

    def test_update_item_unauthorized(self):
        self.client.headers["API_KEY"] = "wrong_key"
        updated_client = {
            "uid": "P000009",
            "code": "sjQ23409A",
            "description": "BABA SHAMPOO XXL 1000ML",
            "short_description": "must",
            "upc_code": "6523540947122",
            "model_number": "63-OFFTq0T",
            "commodity_code": "oTo304",
            "item_line": 11,
            "item_group": 73,
            "item_type": 14,
            "unit_purchase_quantity": 47,
            "unit_order_quantity": 13,
            "pack_order_quantity": 11,
            "supplier_id": 34,
            "supplier_code": "SUP423",
            "supplier_part_number": "E-86805-uTM",
            "created_at": "2015-02-19 16:08:24",
            "updated_at": "2015-09-26 06:37:56"
        }

        response = self.client.put("/items/P000009", json=updated_client)

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)

    def test_delete_item_unauthorized(self):
        self.client.headers["API_KEY"] = "wrong_key"
        response = self.client.delete("/items/P000011")

        self.assertEqual(response.status_code, codes.UNAUTHORIZED)

    def test_get_all(self):
        response = self.client.get("/items")

        self.assertEqual(response.status_code, codes.OK)
        self.assertTrue(len(response.json()) > 0)

    def test_get_single_incorrect(self):
        response = self.client.get("/items/AP000009")

        self.assertEqual(response.status_code, codes.NOT_FOUND)

    def test_get_single(self):
        response = self.client.get("/items/P000009")

        self.assertEqual(response.status_code, codes.OK)
        self.assertIn("uid", response.json())

    def test_create_item(self):
        new_client = {
            "uid": "P000097",
            "code": "saB44409A",
            "description": "Osmanli",
            "short_description": "possible",
            "upc_code": "6523540947122",
            "model_number": "63-OFFTq0T",
            "commodity_code": "oTo304",
            "item_line": 11,
            "item_group": 73,
            "item_type": 14,
            "unit_purchase_quantity": 47,
            "unit_order_quantity": 13,
            "pack_order_quantity": 11,
            "supplier_id": 34,
            "supplier_code": "SUP423",
            "supplier_part_number": "E-86805-uTM",
            "created_at": "2015-02-19 16:08:24",
            "updated_at": "2015-09-26 06:37:56"

        }

        response = self.client.post("/items", json=new_client)
        self.assertEqual(response.status_code, codes.CREATED)

    def test_update_item(self):
        updated_client = {
            "uid": "P000009",
            "code": "sjQ23409A",
            "description": "BABA SHAMPOO XXL 1000ML",
            "short_description": "must",
            "upc_code": "6523540947122",
            "model_number": "63-OFFTq0T",
            "commodity_code": "oTo304",
            "item_line": 11,
            "item_group": 73,
            "item_type": 14,
            "unit_purchase_quantity": 47,
            "unit_order_quantity": 13,
            "pack_order_quantity": 11,
            "supplier_id": 34,
            "supplier_code": "SUP423",
            "supplier_part_number": "E-86805-uTM",
            "created_at": "2015-02-19 16:08:24",
            "updated_at": "2015-09-26 06:37:56"
        }

        response = self.client.put("/items/P000009", json=updated_client)

        self.assertEqual(response.status_code, codes.OK)

    def test_delete_client(self):
        response = self.client.delete("/items/P000096")

        self.assertEqual(response.status_code, codes.OK)

    def test_delete_incorrect_id(self):
        response = self.client.delete("/items/-P000097")

        self.assertEqual(response.status_code, codes.NOT_FOUND)
