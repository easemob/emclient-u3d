package com.easemob.sdk;

import java.io.InputStream;
import java.util.Enumeration;
import java.util.Properties;

import android.content.Context;

public class PropertiesUtil {
	
	public static Properties getProperties(Context context, String fileName)
	{
	    Properties pro = new Properties();
	    try
	    {
	      InputStream is = context.getAssets().open(fileName);
	      if (is == null) {
	        return null;
	      }
	      pro.load(is);
	      return pro;
	    }
	    catch (Exception localException) {}
	    return null;
	}
	
	public static boolean keyExist(Properties properties, String key)
	{
		if(properties == null)
			return false;
		
		if ("".equals(key) || key == null) 
			return false;
		
		Enumeration<?> enu = properties.propertyNames();
		while (enu.hasMoreElements()) {
			String item = (String)enu.nextElement();
			if (item.equals(key)) {
				return true;
			}
		}
		return false; 
	}
	
	public static String getString(Properties properties, String key)
	{
	    if (properties == null) {
	      return null;
	    }
	    return properties.getProperty(key);
	}
	  
	public static int getInt(Properties properties, String key)
	{
	    if (properties == null) {
	      return 0;
	    }
	    String value = properties.getProperty(key);
	    try
	    {
	      return Integer.valueOf(value).intValue();
	    }
	    catch (Exception e)
	    {
	    	e.printStackTrace();
	    }
	    return 0;
	}
	
	public static boolean getBoolean(Properties properties, String key)
	{
		if(properties == null) return false;
		String value = properties.getProperty(key);
		return (value == null) ? false : (Boolean.valueOf(value).booleanValue());
	}
}
