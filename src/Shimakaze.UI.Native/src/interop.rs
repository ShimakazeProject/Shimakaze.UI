pub type GetData = extern "C" fn(str: *const u8, size: usize) -> ResponseData;

#[repr(C)]
pub struct ResponseData {
    content: *mut u8,
    content_size: usize,
    content_type: *mut u8,
    content_type_size: usize,
}

impl ResponseData {
    pub fn is_null(&self) -> bool {
        self.content.is_null() || self.content_type.is_null()
    }

    pub fn get_data(&self) -> Vec<u8> {
        unsafe {
            return Vec::from_raw_parts(self.content, self.content_size, self.content_size);
        }
    }

    pub fn get_content_type(&self) -> String {
        unsafe {
            let vec = Vec::from_raw_parts(
                self.content_type,
                self.content_type_size,
                self.content_type_size,
            );
            String::from_utf8(vec).unwrap()
        }
    }
}
